
using Unity.Mathematics;
using UserTools.Public;

namespace FireSpreading.UserTools {
    public class WindDirectionTool : AbstractScriptableTool {
        public override string ToolName => "Wind Direction";

        public override void OnStart() {
            
        }

        public override void OnValueChanged(float value01) {
            WindGlobals.WIND_DIRECTION = new float3(0, value01 / 360, 0);
        }
    }
}
