using UnityEngine;

namespace Inputs {
    public abstract class AbstractInput : MonoBehaviour {
        public abstract bool IsMouseDown();

        public abstract bool IsMouseActive();

        public abstract Ray GetInputRay();
    }
}
