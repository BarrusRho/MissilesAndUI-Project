using Cinemachine;
using UnityEngine;

namespace GameFeelTest.Utility
{
    public class CameraShake : MonoBehaviour
    {
        private static CameraShake _instance = null;
        public static CameraShake Instance => _instance;
        
        private CinemachineVirtualCamera _virtualCamera;

        private float _shakeIntensity;
        private float _shakeDuration;
        private float _shakeDurationMax;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this. gameObject, 0f);
            }
            
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            if (_shakeDuration > 0)
            {
                _shakeDuration -= Time.deltaTime;

                if (_shakeDuration <= 0f)
                {
                    var basicPerlinShake = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    basicPerlinShake.m_AmplitudeGain = Mathf.Lerp(_shakeIntensity, 0f, (1 - _shakeDuration / _shakeDurationMax));
                }
            }
        }

        public void ShakeCamera(float shakeIntensity, float shakeDuration)
        {
            var basicPerlinShake = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            basicPerlinShake.m_AmplitudeGain = shakeIntensity;
            _shakeIntensity = shakeIntensity;
            _shakeDurationMax = shakeDuration;
            _shakeDuration = shakeDuration;
        }
    }
}
