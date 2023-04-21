using FireSpreading.UserTools.Misc;
using UnityEngine;

namespace FireSpreading.UserTools {
    public sealed class WindSpeedTool : AbstractScriptableTool {
        [SerializeField] private float windSpeedMultiplier = 2f;

        public override string ToolName => "Wind Speed";

        public override void OnStart() {
        }

        public override void OnValueChanged(float value01) {
            WindGlobals.WIND_SPEED = windSpeedMultiplier * value01;
            WindGlobals.WIND_INDICATOR.SetWindSpeed(value01);
        }
    }
}
