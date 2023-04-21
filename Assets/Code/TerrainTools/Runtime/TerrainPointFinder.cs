
using UnityEngine;

namespace TerrainTools {
    public class TerrainPointFinder {
        private static Vector3 positioningOffset = new Vector3(0.5f, 0, 0.5f);

        private readonly TerrainDetails terrainDetails;
        public TerrainPointFinder() {
            terrainDetails = new TerrainDetails();
        }

        public int PositionToIndexOnTerrain (Vector3 position) {
            return PositionToIndex(position, terrainDetails.terrainStartPosition, (int)terrainDetails.terrainSize.x);
        }

        public int PositionToIndex (Vector3 position, Vector3 offset, int sizeX) {
            position -= offset;

            return (int)position.z * sizeX + (int)position.x;
        }

        public Vector3 IndexToPositionOnTerrain (int index) {
            var position = IndexToPosition(index, terrainDetails.terrainStartPosition, (int)terrainDetails.terrainSize.x);
            return position + positioningOffset;
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

