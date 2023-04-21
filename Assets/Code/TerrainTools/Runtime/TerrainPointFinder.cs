
using UnityEngine;

namespace TerrainTools {
    public class TerrainPointFinder {
        private readonly TerrainDimensions terrainDimensions;
        public TerrainPointFinder() {
            terrainDimensions = new TerrainDimensions();
        }

        public int PositionToIndexOnTerrain (Vector3 position) {
            return PositionToIndex(position, terrainDimensions.terrainStartPosition, (int)terrainDimensions.terrainSize.x);
        }

        public int PositionToIndex (Vector3 position, Vector3 offset, int sizeX) {
            position -= offset;

            return (int)position.z * sizeX + (int)position.x;
        }

        public Vector3 IndexToPositionOnTerrain (int index) {
            return IndexToPosition(index, terrainDimensions.terrainStartPosition, (int)terrainDimensions.terrainSize.x);
        }

        public Vector3 IndexToPosition (int index, Vector3 offset, int sizeX) {
            return
                new Vector3(
                    index % sizeX,
                    0,
                    index / sizeX)

                + offset;
        }
    }
}

