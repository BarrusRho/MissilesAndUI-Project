using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using GameFeelTest.Management;
using UnityEngine;
using UnityEngine.UI;

namespace GameFeelTest.UI
{
    public class MediumMissileUI : MonoBehaviour
    {
        [SerializeField] private Button _launchMissileButton;
        [SerializeField] private Transform _buttonTransform;
        
        private float _shakeDuration = 0.5f;
        private float _shakeStrength = 0.5f;
        private float _fadeOutDuration = 1f;
        private float _fadeInDuration = 2f;
        
        private void Awake()
        {
            _launchMissileButton.onClick.AddListener(() =>
            {
                SpawnManager.Instance.GetMediumMissileFromPool();
                ShakeFadeAnimation();
            });
        }

        private async void ShakeFadeAnimation()
        {
            _launchMissileButton.interactable = false;
            
            var tasks = new List<Task>();
            
            tasks.Add(_buttonTransform.DOShakePosition(_shakeDuration, _shakeStrength).AsyncWaitForCompletion());
            tasks.Add(_buttonTransform.DOShakeRotation(_shakeDuration, _shakeStrength).AsyncWaitForCompletion());
            tasks.Add(_buttonTransform.DOShakeScale(_shakeDuration, _shakeStrength).AsyncWaitForCompletion());

            await Task.WhenAll(tasks);

            _buttonTransform.DOScale(Vector3.zero, _fadeOutDuration).SetEase(Ease.Linear);

            await Task.Delay(1000);

            _buttonTransform.DOScale(1f, _fadeInDuration).SetEase(Ease.OutBounce);

            await Task.Delay(1000);

            _launchMissileButton.interactable = true;
        }
    }
}
