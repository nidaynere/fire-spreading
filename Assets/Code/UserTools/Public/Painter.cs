using Dependency;
using Trees;
using UnityEngine.Assertions;

namespace FireSpreading.UserTools.Public {
    public readonly struct Painter {
        private readonly float colorAlpha;
        private readonly TreeRenderer treeRenderer;

        public Painter (float colorAlpha) {
            this.colorAlpha = colorAlpha;

            Assert.IsTrue(ServiceLocator.TryGetSingleton<TreeRenderer>(out treeRenderer));
        }

        public void Run() {
            throw new System.NotImplementedException();

            // use terrain helper to get terrain world position.
            // calculate index by position, and modify.
        }
    }
}
