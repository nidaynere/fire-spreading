


using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Trees.Jobs {
    [BurstCompile]
    public struct GenerateRandomlyJob : IJobParallelFor {
        public NativeArray<byte> results01;

        private readonly float frequency01;
        private readonly int seed;

        public GenerateRandomlyJob(
            NativeArray<byte> results01,
            float frequency01,
            int seed) {
            this.seed = seed;
            this.results01 = results01;
            this.frequency01 = frequency01;
        }

        [BurstCompile]
        public void Execute(int index) {
            if (frequency01 == 0) {
                results01[index] = 0;
                return;
            }

            var random = Random.CreateFromIndex((uint)(seed + index));

            var random01 = random.NextFloat(0f, 1f);

            if (random01 < frequency01) {
                results01[index] = 0;
                return;
            }

            results01[index] = 1;
        }
    }
}

