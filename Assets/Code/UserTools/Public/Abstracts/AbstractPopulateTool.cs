
using Dependency;
using System;
using Trees;
using Trees.Data;
using Trees.Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace FireSpreading.UserTools {
    public abstract class AbstractPopulateTool : AbstractMainTool {
        [Range (0f, 1f)]
        [SerializeField] private float frequency01;

        private TreeRenderer treeRenderer;

        public override void OnValueChanged(float value01) {
            base.OnValueChanged(value01);

            if (value01 < 1) {
                return;
            }

            StartGeneration();
        }

        public override void OnStart() {
            if (!ServiceLocator.TryGetSingleton(out treeRenderer)) {
                throw new Exception("Tree renderer not found.");
            }
        }

        private void StartGeneration() {
            var treeCount = treeRenderer.maxTrees;

            var results01 = new NativeArray<byte>(treeCount, Allocator.Persistent);

            var randomizerJob = new GenerateRandomlyJob(results01, frequency01, Random.Range (0, 1000));

            var jobHandle = randomizerJob.Schedule(treeCount, 1);
            jobHandle.Complete();

            for (var i=0; i<treeCount; i++) {
                var treeEntry = treeRenderer.TreeEntries[i];
                var treeInstance = treeRenderer.TreeInstances[i];

                treeInstance.SetColor (new float4(1, 1, 1, results01[i]));

                treeEntry.Status = results01[i] == 0  ? BurnStatus.Disabled :
                    BurnStatus.Alive;

                treeRenderer.TreeEntries[i] = treeEntry;
                treeRenderer.TreeInstances[i] = treeInstance;
            }

            treeRenderer.RefreshGraphic();

            results01.Dispose();
        }
    }
}
