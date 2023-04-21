
using UnityEngine;

namespace FireSpreading.UserTools {
    public abstract class AbstractPaintTool : AbstractMainTool {
        [SerializeField] private GameObject painter;

        private GameObject instance;

        public override void OnValueChanged(float value01) {
            base.OnValueChanged(value01);

            if (value01 != 1f) {
                Destroy(instance);
                instance = null;
                return;
            }

            if (instance != null) {
                return;
            }

            instance = Instantiate(painter);
        }

        public override void OnStart() {

        }
    }
}
