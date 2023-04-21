
using Trees.Data;

namespace FireSpreading.UserTools {
    public class RemoveTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            treeEntry.Status = BurnStatus.Disabled;
            treeEntry.BurnProgress01 = 0;
            treeInstance.Color = default;
        }
    }
}
