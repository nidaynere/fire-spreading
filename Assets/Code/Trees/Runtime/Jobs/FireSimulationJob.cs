
using Trees.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

namespace Trees.Jobs {
    [BurstCompile]
    public struct FireSimulationJob : IJobParallelFor {
        [NativeDisableParallelForRestriction]
        private NativeArray<TreeData> treeEntries;
        [NativeDisableParallelForRestriction]
        private NativeArray<TreeInstanceData> treeInstances;

        [ReadOnly] private readonly float burnSpeed;
        [ReadOnly] private readonly float spreadingSpeed;
        [ReadOnly] private readonly float deadSpeed;

        [ReadOnly] private readonly int gridSizeX, gridSizeZ;

        [ReadOnly] private readonly int maxIndex;

        [ReadOnly] private readonly float4 burnColor;
        [ReadOnly] private readonly float4 deadColor;

        [ReadOnly] private readonly float3 windDirection;

        public FireSimulationJob(
            NativeArray<TreeData> treeEntries,
            NativeArray<TreeInstanceData> treeInstances,

            int gridSizeX, 
            int gridSizeZ, 
            float3 windDirection,
            float spreadingSpeed,
            float burnSpeed,
            float deadSpeed) {

            burnColor = new float4(1, 0, 0, 1);
            deadColor = new float4(0.2f, 0.2f, 0.2f, 1);

            this.windDirection = math.normalize (windDirection);

            this.burnSpeed = burnSpeed;
            this.spreadingSpeed = spreadingSpeed;
            this.gridSizeX = gridSizeX;
            this.gridSizeZ = gridSizeZ;
            this.treeEntries = treeEntries;
            this.treeInstances = treeInstances;
            this.deadSpeed = deadSpeed;

            maxIndex = gridSizeX * gridSizeZ;
        }

        [BurstCompile]
        private float3 calculatePosition (int index) {
            return new float3(index % gridSizeX, 0, index / gridSizeX);
        }

        [BurstCompile]
        public void Execute(int index) {
                var treeEntry = treeEntries[index];
                var treeInstance = treeInstances[index];

                var myPosition = calculatePosition(index);

                var myX = index % gridSizeX;
                var myY = index / gridSizeZ;

                switch (treeEntry.Status) {
                    case BurnStatus.Alive:
                        bool isNestedLoopKilled = false;
                        // check around if any fire can spread on us.
                        for (var x = -1; x <= 1; x++) {
                            var xIndex = (myX + x) % gridSizeX;

                            if (xIndex >= gridSizeX || xIndex < 0) {
                                continue;
                            }

                            if (isNestedLoopKilled) {
                                break;
                            }

                            for (var y = -1; y <= 1; y++) {
                                var yIndex = myY + y;

                                if (yIndex >= gridSizeZ || yIndex < 0) {
                                    continue;
                                }

                                var final = xIndex + yIndex * gridSizeX;

                                Assert.IsTrue(final < maxIndex && final >= 0, $"Check your algorithm. This shouldnt be possible. xIndex {xIndex} yIndex {yIndex} final {final}");

                                if (treeEntries[final].Status != BurnStatus.Burning) {
                                    continue;
                                }

                                var dirToTarget = math.normalize(myPosition - calculatePosition(final));

                                var dot = math.dot(dirToTarget, windDirection);

                                if (dot < 0.5f) { // 45 angle.
                                    // out of angle.
                                    continue;
                                }

                                treeEntry.Status = BurnStatus.GonnaBurn;
                                treeEntry.BurnProgress01 = 0;
                                isNestedLoopKilled = true;
                                break;
                            }
                        }
                        break;

                    case BurnStatus.GonnaBurn:
                        treeEntry.BurnProgress01 += spreadingSpeed;
                        treeInstance.Color = math.lerp(treeInstance.Color, burnColor, treeEntry.BurnProgress01);
                        if (treeEntry.BurnProgress01 >= 1) {
                            treeEntry.BurnProgress01 = 0;
                            treeEntry.Status = BurnStatus.Burning;
                        }
                        break;

                    case BurnStatus.Burning:
                        treeEntry.BurnProgress01 += burnSpeed;
                        if (treeEntry.BurnProgress01 >= 1) {
                            treeEntry.Status = BurnStatus.Dead;
                            treeEntry.BurnProgress01 = 0;
                        }
                        break;

                    case BurnStatus.Dead:
                        treeEntry.BurnProgress01 += deadSpeed;

                        if (treeEntry.BurnProgress01 >= 1) {
                            treeEntry.Status = BurnStatus.Disabled;
                        }

                        treeInstance.Color = math.lerp(treeInstance.Color, deadColor, treeEntry.BurnProgress01);
                        break;

                    default:
                        break;
                }

                treeEntries[index] = treeEntry;
                treeInstances[index] = treeInstance;
        }
    }
}
