﻿using Dependency;
using Inputs;
using System;
using TerrainTools;
using Trees;
using Trees.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace FireSpreading.UserTools {
    public abstract class AbstractPainterController : MonoBehaviour {
        private TreeRenderer treeRenderer;

        private TerrainRaycaster terrainRaycaster;
        private TerrainPointFinder terrainPointFinder;

        private AbstractInput[] inputs;

        private void Start() {
            if (!ServiceLocator.TryGetSingleton(out treeRenderer)) {
                throw new Exception("Tree renderer not found.");
            }

            inputs = FindObjectsOfType<AbstractInput>();

            terrainRaycaster = new TerrainRaycaster();
            terrainPointFinder = new TerrainPointFinder();
        }

        public void FixedUpdate () {
            foreach (var input in inputs) {
                var ray = input.GetInputRay();

                if (!terrainRaycaster.TryHit(ray, out var point)) {
                    continue;
                }

                var index = terrainPointFinder.PositionToIndexOnTerrain(point);

                var gridPos2D = terrainPointFinder.IndexToPositionOnTerrain(index);
                var gridPos3D = gridPos2D;
                gridPos3D.y = point.y;
                transform.position = gridPos3D;

                if (!input.IsMouseActive()) {
                    continue;
                }

                ModifyTree(index);
            }
        }

        private void ModifyTree(int index) {
            var treeEntry = treeRenderer.TreeEntries[index];
            var treeInstance = treeRenderer.TreeInstances[index];

            OnPaint(ref treeEntry, ref treeInstance);

            treeRenderer.TreeEntries[index] = treeEntry;
            treeRenderer.TreeInstances[index] = treeInstance;

            treeRenderer.RefreshGraphic(index);
        }

        protected abstract void OnPaint(ref TreeData treeEntry, ref TreeInstanceData treeInstance);
    }
}
