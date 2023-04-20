using Trees;
using UnityEngine;
using UnityEngine.Rendering;

namespace FireSpreading {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private ShadowCastingMode shadowCastingMode;
        [SerializeField] private bool receiveShadows;
        [SerializeField] private int maxTrees = 16384;

        private TreeRenderer treeRenderer;

        // Start is called before the first frame update
        void Start() {
            treeRenderer = new TreeRenderer(
                mesh, 
                material, 
                shadowCastingMode, 
                receiveShadows, 
                maxTrees);
        }

        void OnDestroy() {
            treeRenderer.Dispose();
        }
    }
}

