
using UnityEngine;

namespace FireSpreading.UserTools {
    public abstract class PaintTool : AbstractMainTool {
        [SerializeField] private GameObject painter;

        private GameObject instance;

        public override void OnValueChanged(float value01) {
            base.OnValueChanged(value01);

            if (value01 != 1f) {
                Destroy(instance);
                return;
            }

            instance = Instantiate(painter);
        }

        public override void OnStart() {

        }
    }
}
