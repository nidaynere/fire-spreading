using Dependency;
using FireSpreading.UserTools.Misc;
using System;
using TerrainTools;
using Trees;
using Trees.Jobs;
using Unity.Jobs;
using UnityEngine;

namespace FireSpreading.UserTools {
    public class SimulatorController : MonoBehaviour {
        private TreeRenderer treeRenderer;
        private TerrainDetails terrainDetails;

        [SerializeField] private float deadSpeed = 0.2f;
        [SerializeField] private float burnSpeed = 0.3f;
        [SerializeField] private float windSpeed = 1f;

        [Range(0, 90)]
        [SerializeField] private float windDirRandomizerAngle;

        private void Start() {
            if (!ServiceLocator.TryGetSingleton(out treeRenderer)) {
                throw new Exception("Tree renderer not found.");
            }

            terrainDetails = new TerrainDetails();
        }

        private void FixedUpdate() {
            CalculateFireAndSpreading();
        }

        private void CalculateFireAndSpreading() {
            var treeEntries = treeRenderer.TreeEntries;
            var treeInstances = treeRenderer.TreeInstances;

            var fixedDeltaTime = Time.fixedDeltaTime;

            var windDirRandom = Quaternion.Euler(0, UnityEngine.Random.Range(-windDirRandomizerAngle, windDirRandomizerAngle), 0);

            var fireSimulationJob = new FireSimulationJob(
                treeEntries,
                treeInstances,
                (int)terrainDetails.terrainSize.x,
                (int)terrainDetails.terrainSize.z,
                UnityEngine.Random.Range (0, 1000),
                windDirRandom * WindGlobals.WIND_DIRECTION,
                WindGlobals.WIND_SPEED * fixedDeltaTime * windSpeed,
                fixedDeltaTime * burnSpeed,
                fixedDeltaTime * deadSpeed);

            var jobHandle = fireSimulationJob.Schedule(treeRenderer.maxTrees, 1);
            jobHandle.Complete();

            treeRenderer.RefreshInstances();
        }
    }
}
