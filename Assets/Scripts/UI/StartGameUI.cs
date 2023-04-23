using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameFeelTest.UI
{
    public class StartGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject _lowerPanelUI;
        [SerializeField] private RectTransform _startGameUI;
        [SerializeField] private float _fadeTime = 1f;
        [SerializeField] private Button _gameStartButton;

        private void Awake()
        {
            _gameStartButton.onClick.AddListener(() =>
            {
                _lowerPanelUI.SetActive(true);
                PanelFadeOut();
            });
        }
        
        private async void PanelFadeOut()
        {
            var tasks = new List<Task>();
            
            tasks.Add(_startGameUI.DOScale(Vector3.zero, _fadeTime).SetEase(Ease.Linear).AsyncWaitForCompletion());
            
            await Task.WhenAll(tasks);
            
            transform.gameObject.SetActive(false);
        }
    }
}
