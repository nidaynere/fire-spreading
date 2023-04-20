using UnityEngine.UI;

namespace FireSpreading.UserTools.UI {
    public class ButtonTool : AbstractTool<Button> {
        public override Selectable Initialize() {
            base.Initialize();

            uiElementInstance.onClick.AddListener ( () => { scriptableTool.OnValueChanged(1); } );

            return uiElementInstance;
        }
    }
}