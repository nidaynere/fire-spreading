using UnityEngine;

namespace FireSpreading.UserTools {
    public abstract class ScriptableTool : ScriptableObject {
        protected static ScriptableTool activeTool;
        public abstract string ToolName { get; }
        public abstract void OnValueChanged(float value01);
        public abstract void OnStart(); 
    }
}
