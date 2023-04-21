using UnityEngine;
using UnityEngine.UI;

namespace FireSpreading.UserTools.UI {
    public class SliderTool : AbstractTool<Slider> {
        [SerializeField] private float startingValue = 1;
        public override Selectable Initialize() {
            base.Initialize();

            uiElementInstance.onValueChanged.AddListener((value01) => { scriptableTool.OnValueChanged(value01); });

            uiElementInstance.value = startingValue;

            return uiElementInstance;
        }
    }
}