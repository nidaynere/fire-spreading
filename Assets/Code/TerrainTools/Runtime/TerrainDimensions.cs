using UnityEngine;

namespace TerrainTools {
    public class TerrainDimensions {
        public readonly Terrain activeTerrain;
        public readonly Vector3 terrainStartPosition;
        public readonly Vector3 terrainSize;

        /// <summary>
        /// Total grid points count (X size * Y size)
        /// </summary>
        public readonly int totalPoints;

        public TerrainDimensions() {
            activeTerrain = Terrain.activeTerrain;
            terrainStartPosition = activeTerrain.GetPosition();
            terrainSize = activeTerrain.terrainData.size;
            totalPoints = (int)terrainSize.x * (int)terrainSize.z;
        }
    }
}
