
using UnityEngine;

namespace FireSpreading.UserTools {
    public class StartStopSimulationTool : AbstractMainTool {
        [SerializeField] private GameObject simulator;

        private GameObject instance;

        public override string ToolName {
            get => status ? "Stop Simulation" : "Start Simulation";
        }

        private bool m_status;
        private bool status {
            get => m_status;
            set {
                m_status = value;

                UserTool.UpdateTitle();
            }
        }

        public override void OnValueChanged(float value01) {
            base.OnValueChanged(value01);

            if (value01 != 1f || status) {
                status = false;
                Destroy(instance);
                return;
            }

            instance = Instantiate(simulator);
            status = true;
        }

        public override void OnStart() {

        }
    }
}
