using Dependency;
using Inputs;
using TerrainTools;
using Trees;
using UnityEngine;
using UnityEngine.Assertions;

namespace FireSpreading.UserTools.Public {
    public abstract class AbstractPainter : MonoBehaviour {
        protected TreeRenderer treeRenderer;

        private TerrainRaycaster terrainRaycaster;
        private TerrainPointFinder terrainPointFinder;

        private AbstractInput[] inputs;

        private void Start() {
            Assert.IsTrue(ServiceLocator.TryGetSingleton(out treeRenderer));

            inputs = FindObjectsOfType<AbstractInput>();

            terrainRaycaster = new TerrainRaycaster();
            terrainPointFinder = new TerrainPointFinder();
        }

        public void FixedUpdate () {
            foreach (var input in inputs) {
                if (!input.IsMouseActive()) {
                    continue;
                }

                var ray = input.GetInputRay();

                if (!terrainRaycaster.TryHit(ray, out var point)) {
                    continue;
                }

                transform.position = point;

                OnPaint(terrainPointFinder.PositionToIndexOnTerrain(point));
            }
        }

        protected abstract void OnPaint(int index);
    }
}
