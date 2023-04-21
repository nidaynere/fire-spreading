
using UnityEngine;
using UnityEngine.UI;

namespace FireSpreading.UserTools {
    public interface IUserTool {
        public Selectable Initialize();
        public void UpdateTitle();
        public int MenuOrder { get; }
    }
}
