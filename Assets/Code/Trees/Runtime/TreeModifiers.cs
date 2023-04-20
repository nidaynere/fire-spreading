
using TreeData = Trees.Data.TreeData;
using Unity.Collections;
using UnityEngine;

namespace Trees {
    public class TreeModifiers {
        private static readonly Vector3 Off = new Vector3(-5000, -5000, -5000);
        public void RemoveAtIndex (int index, ref NativeArray<TreeData> entries) {
            var entry = entries[index];
            HideTree(ref entry);
            entries[index] = entry;
        }

        public void HideTree (ref TreeData entry) {
            entry.Position = Off;
        }
    }
}
