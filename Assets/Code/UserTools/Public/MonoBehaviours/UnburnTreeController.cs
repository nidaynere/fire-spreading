using Trees.Data;
using UnityEngine;

namespace FireSpreading.UserTools {
    public class UnburnTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            treeEntry.Status = FireSystem.BurnableStatus.Alive;
            treeEntry.BurnProgress01 = 0;

            var currentColor = treeInstance.Color;
            treeInstance.Color = new Color(1, 1, 1, currentColor.a);
        }
    }
}
