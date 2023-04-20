using System;
using Trees.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TreeData = Trees.Data.TreeData;

namespace Trees {
    public class TreeRenderer : IDisposable {

        private static Vector3 positioningOffset = new Vector3(0.5f, 0.5f, 0.5f);

        private MaterialPropertyBlock materialBlockProperty;
        private Bounds bounds;
        private ComputeBuffer instancesBuffer;
        private ComputeBuffer argsBuffer;

        private readonly int maxTrees;
        private readonly Mesh mesh;
        private readonly Material material;

        private readonly ShadowCastingMode shadowCastingMode;
        private readonly bool receiveShadows;

        public NativeArray<TreeData> TreeEntries;
        public TreeInstanceData[] TreeInstances     { get; private set; }

        // Terrain
        private readonly Terrain activeTerrain;
        private readonly Vector3 terrainStartPosition;
        private readonly Vector3 terrainSize;
        //

        public TreeRenderer(
            Mesh mesh, 
            Material material,
            ShadowCastingMode shadowCastingMode,
            bool receiveShadows) {

            // Terrain
            activeTerrain = Terrain.activeTerrain;
            terrainStartPosition = activeTerrain.GetPosition();
            terrainSize = activeTerrain.terrainData.size;
            maxTrees = (int)terrainSize.x * (int)terrainSize.z;
            //

            this.shadowCastingMode = shadowCastingMode;
            this.receiveShadows = receiveShadows;
            this.mesh = mesh;
            this.material = material;

            materialBlockProperty = new MaterialPropertyBlock();
            bounds = new Bounds(Vector3.zero, new Vector3(100000, 100000, 100000));

            TreeEntries = new NativeArray<TreeData>(maxTrees, Allocator.Persistent);

            InitializeBuffers();
        }

        public void Dispose () {
            TreeEntries.Dispose();

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

            for (var i = 0; i < maxTrees; i++) {
                var calculatedTerrainPosition2D = new Vector3(i % (int)terrainSize.z, 0, i / (int)terrainSize.x) + terrainStartPosition;
                var calculatedTerrainPosition3D = calculatedTerrainPosition2D;
                calculatedTerrainPosition3D.y = activeTerrain.SampleHeight(calculatedTerrainPosition2D);

                calculatedTerrainPosition3D += positioningOffset;

                var newTreeData = new TreeData() {
                    Position = calculatedTerrainPosition3D, 
                    rotation = Quaternion.identity,
                    scale = Vector3.one,
                };

                TreeEntries[i] = newTreeData;

                var instance = new TreeInstanceData();
                instance.Matrix = Matrix4x4.TRS(TreeEntries[i].Position, newTreeData.rotation, newTreeData.scale);
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
