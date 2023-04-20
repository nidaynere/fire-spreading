
using UnityEngine.UI;

namespace FireSpreading.UserTools {
    public interface IUserTool {
        public Selectable Initialize();
        public int MenuOrder { get; }
    }
}
