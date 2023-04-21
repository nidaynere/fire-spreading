
using Dependency;
using Trees;
using Trees.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Assertions;

namespace FireSpreading.UserTools {
    public abstract class AbstractPopulateTool : AbstractMainTool {
        [Range (0f, 1f)]
        [SerializeField] private float frequency01;
        public override void OnValueChanged(float value01) {
            base.OnValueChanged(value01);

            if (value01 < 1) {
                return;
            }

            StartGeneration();
        }

        public override void OnStart() {
            
        }

        private void StartGeneration() {
            Assert.IsTrue(ServiceLocator.TryGetSingleton<TreeRenderer>(out var treeRenderer));

            var treeCount = treeRenderer.maxTrees;

            var results01 = new NativeArray<byte>(treeCount, Allocator.Persistent);

            var randomizerJob = new GenerateRandomlyJob(results01, frequency01, (int) (Time.time + Time.deltaTime * 1000));

            var jobHandle = randomizerJob.Schedule(treeCount, 1);
            jobHandle.Complete();

            for (var i=0; i<treeCount; i++) {
                var treeEntry = treeRenderer.treeEntries[i];
                var treeInstance = treeRenderer.TreeInstances[i];

                treeInstance.Color = new Color(1, 1, 1, results01[i]);

                treeEntry.Status = results01[i] == 0  ? FireSystem.BurnableStatus.Disabled :
                    FireSystem.BurnableStatus.Alive;

                treeRenderer.treeEntries[i] = treeEntry;
                treeRenderer.TreeInstances[i] = treeInstance;
            }

            treeRenderer.RefreshInstances();

            results01.Dispose();
        }
    }
}
