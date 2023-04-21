using Trees.Data;
using Unity.Mathematics;

namespace FireSpreading.UserTools {
    public class BurnTreeController : AbstractPainterController {
        protected override void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance) {
            treeEntry.Status = BurnStatus.Burning;
            treeEntry.BurnProgress01 = 0;

            var currentColor = treeInstance.Color;
            treeInstance.Color = new float4(1,1,0,currentColor.w);
        }
    }
}
