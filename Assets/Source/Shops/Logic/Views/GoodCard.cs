using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shops
{
    public abstract class GoodCard : MonoBehaviour, ICard, IDisposable
    {
        [SerializeField] private LeanLocalizedTextMeshProUGUI _title;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _purchaseLabel;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Image _image;
        [SerializeField] private Button _purchase;
        [SerializeField] private GameObject _priceObject;
        [SerializeField] private float _forbidDuration;
        [SerializeField] private float _forbidStrength;

        private CancellationToken _token;
        private RectTransform _purchasePoint;

        public event Action<ICard> Clicked;

        public GoodNames Good { get; private set; }

        public Image Icon => _image;

        public ICard Init(GoodNames good, Sprite icon)
        {
            Good = good;
            _image.sprite = icon;
            _token = destroyCancellationToken;
            _purchasePoint = (RectTransform)_purchase.transform;
            _title.TranslationName = Good.ToString();

            _purchase.onClick.AddListener(OnClick);
            return this;
        }

        public void Dispose()
        {
            _purchase.onClick.RemoveListener(OnClick);
        }

        public void ShowFailure()
        {
            PlayAnimation().Forget();
        }

        public void SetPrice(int value)
        {
            _price.SetText(value.ToString());
        }

        public void HidePrice(Action onHidCallback = null)
        {
            if (_priceObject.activeSelf == false)
                return;

            _priceObject.SetActive(false);
            onHidCallback?.Invoke();
        }

        public void ChangeButtonTranslation(string translationName)
        {
            _purchaseLabel.TranslationName = translationName;
        }

        public void SetButtonInteractable(bool value)
        {
            _purchase.interactable = value;
        }

        public abstract void ShowMaximum(object currentValue = null);

        public abstract void ShowNext((object currentValue, object nextValue, int price) purchase);

        private async UniTaskVoid PlayAnimation()
        {
            SetButtonInteractable(false);
            Color start = _purchase.image.color;

            await UniTask.WhenAll(
                _purchasePoint.DOShakeAnchorPos(_forbidDuration, _forbidStrength).WithCancellation(_token),
                _purchasePoint.DOShakeRotation(_forbidDuration, _forbidStrength).WithCancellation(_token),
                _purchasePoint.DOShakeScale(_forbidDuration, _forbidDuration).WithCancellation(_token),
                _purchase.image.DOColor(Color.red, _forbidDuration / (float)ValueConstants.Two)
                    .WithCancellation(_token));

            await _purchase.image.DOColor(start, _forbidDuration / (float)ValueConstants.Two).WithCancellation(_token);
            SetButtonInteractable(true);
        }

        private void OnClick()
        {
            Clicked?.Invoke(this);
        }
    }
}