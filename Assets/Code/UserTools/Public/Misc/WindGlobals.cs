
using Unity.Mathematics;
using UnityEngine;

namespace FireSpreading.UserTools.Misc {
    public class WindGlobals {
        public static WindIndicator WIND_INDICATOR;
        public static float3 WIND_DIRECTION {
            get => windDirection; 
            set { windDirection = value;
                Shader.SetGlobalVector("WIND_DIRECTION", new float4(value.x, value.y, value.z, 0));
            }
        }

        public static float WIND_SPEED {
            get => windSpeed;
            set {
                windSpeed = value;
                Shader.SetGlobalFloat("WIND_SPEED", value);
            }
        }

        private static float3 windDirection;
        private static float windSpeed;
    }
}
