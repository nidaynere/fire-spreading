
using FireSpreading.FireSystem;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Trees.Data {
    [BurstCompile]
    public struct TreeData : IBurnable {
        public Quaternion rotation;
        public Vector3 scale;

        public float3 Position { get; set; }
        public BurnableStatus Status { get; set; }
        public float BurnProgress01 { get; set; }
    }
}