
using Trees.Data;

namespace FireSpreading.UserTools {
    public class RemoveTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            treeEntry.Status = FireSystem.BurnableStatus.Disabled;
            treeEntry.BurnStartTime = 0;
            treeInstance.Color = default;
        }
    }
}
