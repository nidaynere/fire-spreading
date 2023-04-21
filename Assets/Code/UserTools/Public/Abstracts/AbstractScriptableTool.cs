using UnityEngine;

namespace FireSpreading.UserTools {
    public abstract class AbstractScriptableTool : ScriptableObject {
        protected static AbstractScriptableTool activeTool;

        public IUserTool UserTool { protected get; set; }
        public abstract string ToolName { get; }
        public abstract void OnValueChanged(float value01);
        public abstract void OnStart(); 
    }
}
