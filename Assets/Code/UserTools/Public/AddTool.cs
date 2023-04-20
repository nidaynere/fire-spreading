
namespace FireSpreading.UserTools {
    public class AddTool : AbstractMainTool {
        public override string ToolName => "Add Trees";

        public override void OnValueChanged(float value01) {
            base.OnValueChanged(value01);

            if (value01 != 1f) {
                return;
            }

            // add painter.
        }

        public override void OnStart() {

        }
    }
}
