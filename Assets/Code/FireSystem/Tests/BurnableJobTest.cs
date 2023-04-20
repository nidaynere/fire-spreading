using FireSpreading.FireSystem;
using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.TestTools;

namespace FireSystem.Tests {
    internal class BurnableJobTest {

        internal struct DataSample : IBurnable {
            public Unity.Mathematics.float3 Position { get; set; }
            public BurnableStatus Status { get; set; }
            public float BurnStartTime { get; set; }
        }

        [UnityTest]
        public IEnumerator TrySampleJob() {
            var dataEntries = new NativeArray<DataSample>(16, Allocator.Persistent);
            var results = new NativeArray<int>(16, Allocator.Persistent);

            for (var i=0; i<16; ++i) {
                dataEntries[i] = new DataSample() { Status = (BurnableStatus) Random.Range(0, 3) };
            }

            var testJob = new TestJob() { dataEntries = dataEntries, results = results };

            var handler = testJob.Schedule(16, 1);
            handler.Complete();

            for (var i=0; i<16; ++i) {
                Debug.Log(results[i]);
            }

            dataEntries.Dispose();
            results.Dispose();

            yield break;
        }

        [BurstCompile]
        private struct TestJob : IJobParallelFor {
            [ReadOnly] public NativeArray<DataSample> dataEntries;
            public NativeArray<int> results;

            [BurstCompile]
            public void Execute(int index) {
                results[index] = dataEntries[index].Status == BurnableStatus.Burning ? 1 : 0;
            }
        }
    }
}
