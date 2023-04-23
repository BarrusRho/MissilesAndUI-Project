using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameFeelTest.UI
{
    public class LowerPanelUI : MonoBehaviour
    {
        [SerializeField] private float _fadeTime = 1f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private List<GameObject> _uIObjects = new List<GameObject>();
        
        private void Start()
        {
            PanelFadeIn();
        }

        private void PanelFadeIn()
        {
            _canvasGroup.alpha = 0f;
            _rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
            _rectTransform.DOAnchorPos(new Vector2(0f, 0f), _fadeTime, false).SetEase(Ease.OutElastic);
            _canvasGroup.DOFade(1, _fadeTime);
            StartCoroutine(nameof(UIItemsPopupRoutine));
        }
        
        private IEnumerator UIItemsPopupRoutine()
        {
            foreach (var item in _uIObjects)
            {
                item.transform.localScale = Vector3.zero;
            }

            foreach (var item in _uIObjects)
            {
                item.transform.DOScale(1f, _fadeTime).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
