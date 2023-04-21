using UnityEngine;

namespace TerrainTools {
    public class TerrainRaycaster {
        private readonly TerrainDetails terrainDetails;
        public TerrainRaycaster() {
            terrainDetails = new TerrainDetails();
        }
        public bool TryHit(Ray ray, out Vector3 point) {
            var layer = terrainDetails.terrainSurfaceLayer;

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 1);

            if (Physics.Raycast(
                ray, out var hit, Mathf.Infinity, layer)) {
                point = hit.point;
                return true;
            }

            point = Vector3.zero;
            return false;
        }
    }
}
