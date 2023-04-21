using Dependency;
using TerrainTools;
using Trees;
using Trees.Jobs;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Assertions;
using UserTools.Public;

namespace FireSpreading.UserTools {
    public class SimulatorController : MonoBehaviour {
        private TreeRenderer treeRenderer;
        private TerrainDetails terrainDetails;

        private void Start() {
            Assert.IsTrue(ServiceLocator.TryGetSingleton(out treeRenderer));

            terrainDetails = new TerrainDetails();
        }

        private void FixedUpdate() {
            var queryLength = terrainDetails.totalPoints;

            var treeEntries = treeRenderer.TreeEntries;
            var treeInstances = treeRenderer.TreeInstances;

            var fireSimulationJob = new FireSimulationJob(
                treeEntries,
                treeInstances,
                (int)terrainDetails.terrainSize.x,
                (int)terrainDetails.terrainSize.z,
                WindGlobals.WIND_DIRECTION,
                WindGlobals.WIND_SPEED,
                1);

            var jobHandle = fireSimulationJob.Schedule(queryLength, 1);
            jobHandle.Complete();

            treeRenderer.RefreshInstances();
        }
    }
}
