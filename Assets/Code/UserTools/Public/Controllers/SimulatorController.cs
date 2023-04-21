using Dependency;
using FireSpreading.UserTools.Misc;
using TerrainTools;
using Trees;
using Trees.Jobs;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Assertions;

namespace FireSpreading.UserTools {
    public class SimulatorController : MonoBehaviour {
        private TreeRenderer treeRenderer;
        private TerrainDetails terrainDetails;

        private void Start() {
            Assert.IsTrue(ServiceLocator.TryGetSingleton(out treeRenderer));

            terrainDetails = new TerrainDetails();
        }

        private void FixedUpdate() {
            CalculateFireAndSpreading();
        }

        private void CalculateFireAndSpreading() {
            var treeEntries = treeRenderer.TreeEntries;
            var treeInstances = treeRenderer.TreeInstances;

            float fixedDeltaTime = Time.fixedDeltaTime;

            var fireSimulationJob = new FireSimulationJob(
                treeEntries,
                treeInstances,
                (int)terrainDetails.terrainSize.x,
                (int)terrainDetails.terrainSize.z,
                WindGlobals.WIND_DIRECTION,
                WindGlobals.WIND_SPEED * fixedDeltaTime,
                fixedDeltaTime * 1);

            var jobHandle = fireSimulationJob.Schedule();
            jobHandle.Complete();

            treeRenderer.RefreshInstances();
        }
    }
}
