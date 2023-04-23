using Dependency;
using FireSpreading.UserTools.Misc;
using System;
using TerrainTools;
using Trees;
using Trees.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FireSpreading.UserTools {
    public class SimulatorController : MonoBehaviour {
        private TreeRenderer treeRenderer;
        private TerrainDetails terrainDetails;

        [SerializeField] private float deadSpeed = 0.2f;
        [SerializeField] private float burnSpeed = 0.3f;
        [SerializeField] private float windSpeed = 1f;

        [Range(0, 90)]
        [SerializeField] private float windDirRandomizerAngle = 45f;

        [Range(0f, 1f)]
        [SerializeField] private float randomWindChance01 = 0.05f;

        [SerializeField] private int maxUpdateVisuals = 32;

        private NativeArray<byte> updateVisualOrderArray;
        private int updateVisualOrderArrayLength;
        private int updateVisualOrderIndex;

        private void Start() {
            if (!ServiceLocator.TryGetSingleton(out treeRenderer)) {
                throw new Exception("Tree renderer not found.");
            }

            terrainDetails = new TerrainDetails();

            updateVisualOrderArray = new NativeArray<byte>(treeRenderer.maxTrees, Allocator.Persistent);
            updateVisualOrderArrayLength = treeRenderer.maxTrees;
        }

        private void OnDestroy() {
            updateVisualOrderArray.Dispose();
        }

        private void FixedUpdate() {
            CalculateFireAndSpreading();
        }

        private void CalculateFireAndSpreading() {
            var treeEntries = treeRenderer.TreeEntries;
            var treeInstances = treeRenderer.TreeInstances;

            var fixedDeltaTime = Time.fixedDeltaTime;

            var windDirRandom = 
                Random.Range (0f, 1f) < randomWindChance01 ? 
                Quaternion.Euler(0, Random.Range(-windDirRandomizerAngle, windDirRandomizerAngle), 0) : 
                Quaternion.identity;

            var fireSimulationJob = new FireSimulationJob(
                treeEntries,
                treeInstances,
                updateVisualOrderArray,
                (int)terrainDetails.terrainSize.x,
                (int)terrainDetails.terrainSize.z,
                Random.Range (0, 1000),
                windDirRandom * WindGlobals.WIND_DIRECTION,
                WindGlobals.WIND_SPEED * fixedDeltaTime * windSpeed,
                fixedDeltaTime * burnSpeed,
                fixedDeltaTime * deadSpeed);

            var jobHandle = fireSimulationJob.Schedule(treeRenderer.maxTrees, 16);
            jobHandle.Complete();

            var updated = 0;

            while (updateVisualOrderIndex < updateVisualOrderArrayLength) {
                var i = updateVisualOrderIndex;

                if (++updateVisualOrderIndex >= updateVisualOrderArrayLength) {
                    updateVisualOrderIndex = 0;
                    break;
                }

                if (updateVisualOrderArray[i] == 0) {
                    continue;
                }

                if (++updated > maxUpdateVisuals) {
                    break; // out of limit.
                }

                updateVisualOrderArray[i] = 0;

                treeRenderer.RefreshGraphic(i);
            }
        }
    }
}
