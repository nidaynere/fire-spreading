
using Trees.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace Trees.Jobs {
    [BurstCompile]
    public struct FireSimulationJob : IJobParallelFor {
        private NativeArray<TreeData> treeEntries;
        private NativeArray<TreeInstanceData> treeInstances;

        private readonly float burnSpeed;
        private readonly float spreadingSpeed;
        private readonly int gridSizeX, gridSizeZ;

        private readonly int maxIndex;

        private readonly float4 burnColor;
        private readonly float4 deadColor;

        private readonly float3 windDirection;

        public FireSimulationJob(
            NativeArray<TreeData> treeEntries,
            NativeArray<TreeInstanceData> treeInstances,

            int gridSizeX, 
            int gridSizeZ, 
            float3 windDirection,
            float spreadingSpeed,
            float burnSpeed) {

            burnColor = new float4(1, 0, 0, 1);
            deadColor = new float4(0, 0, 0, 1);

            this.windDirection = math.normalize (windDirection);

            this.burnSpeed = burnSpeed;
            this.spreadingSpeed = spreadingSpeed;
            this.gridSizeX = gridSizeX;
            this.gridSizeZ = gridSizeZ;
            this.treeEntries = treeEntries;
            this.treeInstances = treeInstances;

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

            switch (treeEntry.Status) {
                case BurnStatus.Alive:
                    bool isNestedLoopKilled = false;
                    // check around if any fire can spread on us.
                    for (var x = -1; x <= 1; x++) {
                        if (isNestedLoopKilled) {
                            break;
                        }

                        for (var y = -1; y <= 1; y++) {
                            if (x >= gridSizeX) {
                                continue;
                            }

                            if (y >= gridSizeZ) {
                                continue;
                            }

                            var xIndex = (index + x) % gridSizeX;
                            var yIndex = index / gridSizeZ + y;

                            var final = xIndex + yIndex * gridSizeX;

                            Assert.IsTrue(final < maxIndex, "Check your algorithm. This shouldnt be possible.");

                            var dirToTarget = math.normalize (calculatePosition(final) - myPosition);

                            if (math.dot(dirToTarget, windDirection) < 0) {
                                // out of angle.
                                continue;
                            }

                            treeEntry.BurnProgress01 += spreadingSpeed;

                            if (treeEntry.BurnProgress01 >= 0) {
                                isNestedLoopKilled = true;

                                treeEntry.BurnProgress01 = 0;
                                treeEntry.Status = BurnStatus.GonnaBurn;
                                break;
                            }
                        }
                    }
                    break;

                case BurnStatus.GonnaBurn:
                    treeEntry.BurnProgress01 += spreadingSpeed;
                    treeInstance.Color = math.lerp(treeInstance.Color, burnColor, treeEntry.BurnProgress01);
                    if (treeEntry.BurnProgress01 >= 0) {
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
                    treeEntry.BurnProgress01 += burnSpeed;

                    if (treeEntry.BurnProgress01 >= 1) {
                        treeEntry.Status = BurnStatus.Disabled;
                    }

                    treeInstance.Color = math.lerp(treeInstance.Color, deadColor, treeEntry.BurnProgress01);
                    break;

                default:
                    return;
            }
        }
    }

}
