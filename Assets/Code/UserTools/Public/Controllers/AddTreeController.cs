using Trees.Data;
using Unity.Mathematics;
using UnityEngine;

namespace FireSpreading.UserTools {
    public class AddTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            if (treeInstance.Color.w > 0) {
                return; // tree is already visible.
            }

            treeEntry.Status = BurnStatus.Alive;
            treeEntry.BurnProgress01 = 0;
            treeInstance.Color = new float4 (1,1,1,1);
        }
    }
}
