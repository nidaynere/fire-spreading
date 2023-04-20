
using TreeData = Trees.Data.TreeData;
using Unity.Collections;

namespace Trees {
    public class TreeModifiers {
        private static readonly float OffYPoint = -float.MaxValue;
        public void RemoveAtIndex (int index, ref NativeArray<TreeData> entries) {
            var entry = entries[index];
            HideTree(ref entry);
            entries[index] = entry;
        }

        public void HideTree (ref TreeData entry) {
            var position = entry.Position;
            position.y = OffYPoint;
            entry.Position = position;
        }
    }
}
