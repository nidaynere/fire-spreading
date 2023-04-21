using UnityEngine;

namespace FireSpreading.UserTools {
    public abstract class AbstractMainTool : AbstractScriptableTool {
        public override void OnValueChanged(float value01) {
            if (value01 == 0) {
                return;
            }

            if (activeTool != null && activeTool != this) {
                activeTool.OnValueChanged(0);
            }

            activeTool = this;
        }
    }
}
