using Unity.Burst;
using Unity.Mathematics;

namespace Trees.Data {
    [BurstCompile]
    public struct TreeInstanceData {
        public float4x4 Matrix;
        public float4x4 MatrixInverse;

        public float TransitionStartTime;
        public float4 Color;
        public float4 TargetColor;

        public static int Size() {
            return sizeof(float) * 4 * 4
                 + sizeof(float) * 4 * 4
                 + sizeof(float) * 4
                 + sizeof(float) * 4
                 + sizeof(float);
        }

        public void ColorInTime (in float time, float4 color) {
            TransitionStartTime = time;
            Color = TargetColor;
            TargetColor = color;
        }

        public void SetColor (float4 color) {
            Color = TargetColor = color;
            TransitionStartTime = 0;
        }
    }
}
