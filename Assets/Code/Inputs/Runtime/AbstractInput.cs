using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Inputs {
    public abstract class AbstractInput : MonoBehaviour {
        public abstract bool IsMouseDown();

        public abstract bool IsMouseActive();

        public abstract Vector2 MousePosition();
    }
}
