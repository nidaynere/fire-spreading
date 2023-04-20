using FireSpreading.UserTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace FireSpreading.UserTools {
    public class UserToolUIInitializer : MonoBehaviour {
        private void Start() {
            var tools = transform.GetComponentsInChildren<IUserTool>(true);

            var toolsLength = tools.Length;

            var instancedTools = new Selectable[toolsLength];

            for (var i=0; i<toolsLength; i++) {
                instancedTools[i] = tools[i].Initialize();
            }

            for (var i=toolsLength-1; i>=0; i--) {
                instancedTools[i].transform.SetSiblingIndex(tools[i].MenuOrder);
            }
        }
    }
}
