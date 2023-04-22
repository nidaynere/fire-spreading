
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Trees.Data {
    [BurstCompile]
    public struct TreeData {
        public float3 Position { get; set; }
        public BurnStatus Status { get; set; }
        public float BurnProgress01 { get; set; }
    }
}