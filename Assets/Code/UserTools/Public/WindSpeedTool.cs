using FireSpreading.UserTools.Misc;
using UnityEngine;

namespace FireSpreading.UserTools {
    public sealed class WindSpeedTool : AbstractScriptableTool {
        public override string ToolName => "Wind Speed";

        public override void OnStart() {
        }

        public override void OnValueChanged(float value01) {
            WindGlobals.WIND_SPEED = value01;
            WindGlobals.WIND_INDICATOR.SetWindSpeed(value01);
        }
    }
}
