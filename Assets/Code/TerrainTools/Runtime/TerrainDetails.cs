using UnityEngine;

namespace TerrainTools {
    public class TerrainDetails {
        public readonly Terrain activeTerrain;
        public readonly Vector3 terrainStartPosition;
        public readonly Vector3 terrainSize;
        public readonly LayerMask terrainSurfaceLayer;

        /// <summary>
        /// Total grid points count (X size * Y size)
        /// </summary>
        public readonly int totalPoints;

        public TerrainDetails() {
            activeTerrain = Terrain.activeTerrain;
            terrainStartPosition = activeTerrain.GetPosition();
            terrainSize = activeTerrain.terrainData.size;
            totalPoints = (int)terrainSize.x * (int)terrainSize.z;

            terrainSurfaceLayer = 1 << activeTerrain.gameObject.layer;
        }
    }
}
