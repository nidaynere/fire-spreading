using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs {
    public class StandaloneInput : AbstractInput {
        [SerializeField] private InputActionReference mouseClickReference;
        [SerializeField] private InputActionReference mousePositionReference;

        public override bool IsMouseActive() {
            throw new System.NotImplementedException();
        }

        public override bool IsMouseDown() {
            throw new System.NotImplementedException();
        }

        public override Vector2 MousePosition() {
            throw new System.NotImplementedException();
        }
    }
}

