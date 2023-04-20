using UnityEngine;

namespace FireSpreading.UserTools {
    public abstract class AbstractMainTool : ScriptableTool {
        public override void OnValueChanged(float value01) {
            Debug.Log($"[AbstractMainTool] OnValueChanged {GetType()} => {value01}");

            if (value01 == 0) {
                return;
            }

            if (activeTool != null) {
                activeTool.OnValueChanged(0);
            }

            activeTool = this;
        }
    }
}
