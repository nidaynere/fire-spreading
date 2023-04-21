using Unity.Burst;
using Unity.Mathematics;

namespace Trees.Data {
    [BurstCompile]
    public struct TreeInstanceData {
        public float4x4 Matrix;
        public float4x4 MatrixInverse;
        public float4 Color;

        public static int Size() {
            return sizeof(float) * 4 * 4
                 + sizeof(float) * 4 * 4
                 + sizeof(float) * 4;
        }
    }
}
