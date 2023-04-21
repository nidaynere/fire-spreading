using Unity.Mathematics;
using UnityEngine;

namespace FireSpreading.UserTools.Misc {
    public class WindIndicator : MonoBehaviour {
        [SerializeField] private Transform windPowerIndicator;
        public void SetWindSpeed (float power01) {
            windPowerIndicator.localScale = new Vector3(1, 1, power01);
        }

        public void SetWindDirection (float3 direction) {
            transform.rotation = Quaternion.LookRotation (direction);
        }
    }
}
