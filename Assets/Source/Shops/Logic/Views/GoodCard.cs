using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        private Action _onClickCallback;
        private CancellationToken _token;
        private RectTransform _purchasePoint;

        public GoodNames Good { get; private set; }

        public void Init(GoodNames good, Action onClickCallback)
        {
            Good = good;
            _onClickCallback = onClickCallback;
            _token = destroyCancellationToken;
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
            PlayAnimation().Forget();
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

        private async UniTaskVoid PlayAnimation()
        {
            SetButtonInteractable(false);
            Color start = _purchase.image.color;

            await UniTask.WhenAll(
                _purchasePoint.DOShakeAnchorPos(_forbidDuration).WithCancellation(_token),
                _purchasePoint.DOShakeRotation(_forbidDuration, _forbidStrength).WithCancellation(_token),
                _purchasePoint.DOShakeScale(_forbidDuration).WithCancellation(_token),
                _purchase.image.DOColor(Color.red, _forbidDuration / (float)ValueConstants.Two)
                    .WithCancellation(_token));

            await _purchase.image.DOColor(start, _forbidDuration / (float)ValueConstants.Two).WithCancellation(_token);
            SetButtonInteractable(true);
        }
    }
}