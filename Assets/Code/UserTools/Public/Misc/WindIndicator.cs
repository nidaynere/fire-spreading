using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace FireSpreading.UserTools.Misc {
    public class WindIndicator : MonoBehaviour {
        [SerializeField] private Transform windIndicatorRotator;
        [SerializeField] private Image windPowerIndicatorImage;

        private Transform mainCameraTransform;

        private void Start() {
            mainCameraTransform = Camera.main.transform;
        }

        public void SetWindSpeed (float power01) {
            windPowerIndicatorImage.fillAmount = power01;
        }

        private void LateUpdate() {
            var cameraY = mainCameraTransform.eulerAngles.y;
            var windRot = Quaternion.LookRotation(WindGlobals.WIND_DIRECTION);

            var windDir =  Quaternion.Euler (0, 0, -windRot.eulerAngles.y) * Quaternion.Euler (0, 0, cameraY);
            windIndicatorRotator.rotation = windDir;
        }
    }
}
