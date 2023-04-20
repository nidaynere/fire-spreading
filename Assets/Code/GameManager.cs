using Dependency;
using Trees;
using UnityEngine;
using UnityEngine.Rendering;

namespace FireSpreading {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private ShadowCastingMode shadowCastingMode;
        [SerializeField] private bool receiveShadows;

        private TreeRenderer treeRenderer;

        // Start is called before the first frame update
        void Start() {
            treeRenderer = new TreeRenderer(
                mesh, 
                material, 
                shadowCastingMode, 
                receiveShadows);

            ServiceLocator.RegisterSingleton(treeRenderer);
        }

        void OnDestroy() {
            treeRenderer.Dispose();
        }

        void Update() {
            treeRenderer.Draw();
        }
    }
}

