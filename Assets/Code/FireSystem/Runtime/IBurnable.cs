using Unity.Mathematics;

namespace FireSpreading.FireSystem {
    public interface IBurnable {
        public float3               Position { get; set; }
        public BurnableStatus       Status { get; set; }
        public float                BurnStartTime { get; set; }
    }
}
