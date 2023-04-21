using System;
using Trees.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TreeData = Trees.Data.TreeData;
using TerrainTools;

namespace Trees {
    public class TreeRenderer : IDisposable {
        private static Vector3 positioningOffset = new Vector3(0.5f, 0, 0.5f);

        private MaterialPropertyBlock materialBlockProperty;
        private Bounds bounds;
        private ComputeBuffer instancesBuffer;
        private ComputeBuffer argsBuffer;

        private readonly Mesh mesh;
        private readonly Material material;

        private readonly ShadowCastingMode shadowCastingMode;
        private readonly bool receiveShadows;

        private readonly TerrainDetails terrainDetails;
        private readonly TerrainPointFinder terrainPointFinder;

        public readonly int maxTrees;

        public NativeArray<TreeData> treeEntries;
        public TreeInstanceData[] TreeInstances     { get; private set; }

        public TreeRenderer(
            Mesh mesh, 
            Material material,
            ShadowCastingMode shadowCastingMode,
            bool receiveShadows) {

            terrainDetails = new TerrainDetails();
            terrainPointFinder = new TerrainPointFinder();

            maxTrees = terrainDetails.totalPoints;

            this.shadowCastingMode = shadowCastingMode;
            this.receiveShadows = receiveShadows;
            this.mesh = mesh;
            this.material = material;

            materialBlockProperty = new MaterialPropertyBlock();
            bounds = new Bounds(Vector3.zero, new Vector3(10000, 10000, 10000));

            treeEntries = new NativeArray<TreeData>(maxTrees, Allocator.Persistent);

            InitializeBuffers();
        }

        public void Dispose () {
            treeEntries.Dispose();

            if (instancesBuffer != null) {
                instancesBuffer.Release();
                instancesBuffer = null;
            }

            if (argsBuffer != null) {
                argsBuffer.Release();
                argsBuffer = null;
            }
        }

        private ComputeBuffer GetArgsBuffer(uint count) {
            uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
            args[0] = (uint)mesh.GetIndexCount(0);
            args[1] = (uint)count;
            args[2] = (uint)mesh.GetIndexStart(0);
            args[3] = (uint)mesh.GetBaseVertex(0);
            args[4] = 0;

            ComputeBuffer buffer = new ComputeBuffer(args.Length, sizeof(uint), ComputeBufferType.IndirectArguments);
            buffer.SetData(args);
            return buffer;
        }

        private void InitializeBuffers() {
            TreeInstances = new TreeInstanceData [maxTrees];

            var terrainSize = terrainDetails.terrainSize;
            var terrainStartPosition = terrainDetails.terrainStartPosition;
            var activeTerrain = terrainDetails.activeTerrain;

            for (var i = 0; i < maxTrees; i++) {
                var calculatedTerrainPosition2D = terrainPointFinder.IndexToPositionOnTerrain (i);
                var calculatedTerrainPosition3D = calculatedTerrainPosition2D;
                calculatedTerrainPosition3D += positioningOffset;
                calculatedTerrainPosition3D.y = activeTerrain.SampleHeight(calculatedTerrainPosition2D);

                var newTreeData = new TreeData() {
                    Position = calculatedTerrainPosition3D,

                    rotation = Quaternion.identity,
                    scale = Vector3.one,
                };

                treeEntries[i] = newTreeData;

                var instance = new TreeInstanceData();
                instance.Matrix = Matrix4x4.TRS(treeEntries[i].Position, newTreeData.rotation, newTreeData.scale);
                instance.MatrixInverse = instance.Matrix.inverse;
                instance.Color = Color.white;
                TreeInstances[i] = instance;
            }

            argsBuffer = GetArgsBuffer((uint)maxTrees);
            instancesBuffer = new ComputeBuffer(maxTrees, TreeInstanceData.Size());

            RefreshInstances();
        }

        public void RefreshInstances () {
            instancesBuffer.SetData(TreeInstances);
            material.SetBuffer("_PerInstanceItemData", instancesBuffer);
        }

        public void Draw () {
            Graphics.DrawMeshInstancedIndirect(
                mesh,
                0, 
                material, 
                bounds, 
                argsBuffer, 
                0, 
                materialBlockProperty, 
                shadowCastingMode, 
                receiveShadows);
        }
    }
}
