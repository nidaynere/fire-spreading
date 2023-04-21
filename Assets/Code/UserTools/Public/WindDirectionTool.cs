
using FireSpreading.UserTools.Misc;
using Unity.Mathematics;
using UnityEngine;

namespace FireSpreading.UserTools {
    public class WindDirectionTool : AbstractScriptableTool {
        public override string ToolName => "Wind Direction";

        [SerializeField] private WindIndicator windDirectionIndicator;

        public override void OnStart() {
            WindGlobals.WIND_INDICATOR = Instantiate(windDirectionIndicator);
        }

        public override void OnValueChanged(float value01) {
            var windAngle = Quaternion.Euler (0, value01 * 360, 0);
            WindGlobals.WIND_DIRECTION = windAngle * Vector3.forward;
        }
    }
}
