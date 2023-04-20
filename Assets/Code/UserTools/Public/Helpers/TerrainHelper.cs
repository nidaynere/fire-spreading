
using Inputs;
using UnityEngine;

namespace FireSpreading.UserTools.Helpers {
    public class TerrainHelper {
        private readonly AbstractInput[] inputs;

        public TerrainHelper() {
            inputs = Object.FindObjectsOfType<AbstractInput>();
        }

        protected Vector3 FindPositionOnTerrainByInput () {
            foreach (var input in inputs) {
                var mousePosition = input.MousePosition();

                // screen to world, and cast on terrain, return pos.
            }

            throw new System.NotImplementedException();
        }
    }
}
