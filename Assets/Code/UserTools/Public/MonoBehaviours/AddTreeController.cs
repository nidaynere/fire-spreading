using Trees.Data;
using UnityEngine;

namespace FireSpreading.UserTools {
    public class AddTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            treeEntry.Status = FireSystem.BurnableStatus.Alive;
            treeEntry.BurnStartTime = 0;
            treeInstance.Color = Color.white;
        }
    }
}
