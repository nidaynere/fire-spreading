using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FireSpreading.UserTools.UI {
    public abstract class AbstractTool<T> : MonoBehaviour, IUserTool where T : Selectable {
        [SerializeField] private T uiElement;
        [SerializeField] private Transform uiHolder;
        [SerializeField] protected AbstractScriptableTool scriptableTool;

        protected T uiElementInstance;

        int IUserTool.MenuOrder => transform.GetSiblingIndex();

        public virtual Selectable Initialize () {
            uiElementInstance = Instantiate(uiElement, uiHolder); 

            var toolText = uiElementInstance.GetComponentInChildren<TextMeshProUGUI>(true);
            Assert.IsNotNull(toolText, $"Ui element should have at least one {typeof (TextMeshProUGUI)} component inside.");

            toolText.text = scriptableTool.ToolName;

            scriptableTool.OnStart();

            return uiElementInstance;
        }
    }
}
