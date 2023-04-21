
using UnityEngine.EventSystems;

namespace Inputs {
    internal class InputHelper {
        //Returns 'true' if we touched or hovering on Unity UI element.
        public bool IsPointerOverUIElement() {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
