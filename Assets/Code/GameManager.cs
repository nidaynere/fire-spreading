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

        [SerializeField] private Vector2 randomRotation = new Vector2(0, 360);
        [SerializeField] private Vector2 randomScale = new Vector2(0.75f, 1.25f);
        [SerializeField] private float baseScale = 2f;

        private TreeRenderer treeRenderer;

        // Start is called before the first frame update
        void Awake() {
            treeRenderer = new TreeRenderer(
                mesh, 
                material, 
                shadowCastingMode, 
                receiveShadows,
                baseScale,
                randomRotation,
                randomScale);

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

