using System;
using DG.Tweening;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Shops
{
    public abstract class GoodCard : MonoBehaviour, ICard, IDisposable
    {
        [SerializeField] private LeanLocalizedTextMeshProUGUI _title;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _purchaseLabel;
        [SerializeField] private Button _purchase;
        [SerializeField] private GameObject _price;
        [SerializeField] private VerticalLayoutGroup _layout;
        [SerializeField] private float _forbidDuration;
        [SerializeField] private float _forbidStrength;

        private RectTransform _purchasePoint;
        private Action _onClickCallback;

        public GoodNames Good { get; private set; }

        public void Init(GoodNames good, Action onClickCallback)
        {
            Good = good;
            _onClickCallback = onClickCallback;
            _purchasePoint = (RectTransform)_purchase.transform;
            _title.TranslationName = Good.ToString();
            _layout.enabled = false;
            _purchase.onClick.AddListener(_onClickCallback.Invoke);
        }

        public void Dispose()
        {
            _purchase.onClick.RemoveListener(_onClickCallback.Invoke);
        }

        public void ShowFailure()
        {
            _purchasePoint.DOShakeAnchorPos(_forbidDuration);
            _purchasePoint.DOShakeRotation(_forbidDuration, _forbidStrength);
            _purchasePoint.DOShakeScale(_forbidDuration);
            Color start = _purchase.image.color;
            _purchase.image.DOColor(Color.red, _forbidDuration / (float)ValueConstants.Two).OnComplete(
                () => _purchase.image.DOColor(start, _forbidDuration / (float)ValueConstants.Two));
        }

        public void HidePrice()
        {
            _layout.enabled = true;
            _price.SetActive(false);
            _layout.enabled = false;
        }

        public void ChangeButtonTranslation(string translationName)
        {
            _purchaseLabel.TranslationName = translationName;
        }

        public void SetButtonInteractable(bool value)
        {
            _purchase.interactable = value;
        }

        public abstract void ShowMaximum();

        public abstract void ShowNext((object currentValue, object nextValue, int price) purchase);
    }
}