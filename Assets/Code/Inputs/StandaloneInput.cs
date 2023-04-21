using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Inputs {
    public class StandaloneInput : AbstractInput {
        [SerializeField] private InputActionReference mouseClickReference;
        [SerializeField] private InputActionReference mousePositionReference;

        private InputHelper inputHelper;

        private void Start() {
            inputHelper = new InputHelper();
        }

        public override bool IsMouseActive() {
            if (inputHelper.IsPointerOverUIElement ()) {
                return false;
            }

            return mouseClickReference.action.IsPressed ();
        }

        public override bool IsMouseDown() {
            if (inputHelper.IsPointerOverUIElement()) {
                return false;
            }

            return mouseClickReference.action.WasPressedThisFrame();
        }

        public override Ray GetInputRay() {
            var camera = Camera.main;

            var near = camera.nearClipPlane;

            var mousePosition = mousePositionReference.action.ReadValue<Vector2>();

            Vector3 actualPos = mousePosition;
            actualPos.z = near;

            var ray = Camera.main.ScreenPointToRay(actualPos);

            return ray;
        }
    }
}

