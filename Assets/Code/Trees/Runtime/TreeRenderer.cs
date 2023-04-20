using System;
using Trees.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TreeData = Trees.Data.TreeData;

namespace Trees {
    public class TreeRenderer : IDisposable {
        private MaterialPropertyBlock materialBlockProperty;
        private Bounds bounds;
        private ComputeBuffer instancesBuffer;
        private ComputeBuffer argsBuffer;

        private NativeArray<TreeData> treeData;

        public TreeInstanceData[] TreeInstances { get; private set; }

        private readonly int maxTrees;
        private readonly Mesh mesh;
        private readonly Material material;

        private readonly TreeModifiers treeModifiers;

        private readonly ShadowCastingMode shadowCastingMode;
        private readonly bool receiveShadows;

        public TreeRenderer(
            Mesh mesh, 
            Material material,
            ShadowCastingMode shadowCastingMode,
            bool receiveShadows,
            int maxTrees = 1024) {
            
            this.shadowCastingMode = shadowCastingMode;
            this.receiveShadows = receiveShadows;
            this.maxTrees = maxTrees;
            this.mesh = mesh;
            this.material = material;

            treeModifiers = new TreeModifiers();

            materialBlockProperty = new MaterialPropertyBlock();
            bounds = new Bounds(Vector3.zero, new Vector3(100000, 100000, 100000));

            treeData = new NativeArray<TreeData>(maxTrees, Allocator.Persistent);

            InitializeBuffers();
        }

        public void Dispose () {
            treeData.Dispose();

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
                var newTreeData = new TreeData() { rotation = Quaternion.identity, scale = Vector3.one };
                treeModifiers.HideTree(ref newTreeData);
                treeData[i] = newTreeData;

                var instance = new TreeInstanceData();
                instance.Matrix = Matrix4x4.TRS(treeData[i].Position, newTreeData.rotation, newTreeData.scale);
                TreeInstances[i] = instance;
            }

            argsBuffer = GetArgsBuffer((uint)maxTrees);
            instancesBuffer = new ComputeBuffer(maxTrees, TreeInstanceData.Size());
        }

        public void RefreshData () {
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
