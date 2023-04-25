using Trees.Data;
using Unity.Mathematics;

namespace FireSpreading.UserTools {
    public class UnburnTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            treeEntry.Status = BurnStatus.Alive;
            treeEntry.BurnProgress01 = 0;

            var currentColor = treeInstance.Color;
            treeInstance.SetColor (new float4(1, 1, 1, currentColor.w));
        }
    }
}
