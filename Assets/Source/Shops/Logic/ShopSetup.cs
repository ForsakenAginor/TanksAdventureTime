using System;
using System.Collections.Generic;
using System.Linq;
using PlayerHelpers;
using UnityEngine;

namespace Shops
{
    public class ShopSetup : MonoBehaviour
    {
        [SerializeField] private Goods _goods;
        [SerializeField] private RectTransform _holder;
        [SerializeField] private List<GoodCard> _cardTemplates;

        private Shop _model;
        private ShopView _view;
        private ShopPresenter _presenter;
        private ICardFactory _factory;

        private void OnDestroy()
        {
            _presenter?.Disable();
        }

        public void Init(
            IWallet wallet,
            Action<Purchases> purchaseChangeCallback,
            Action<int> selectionCallback,
            IReadOnlyCharacteristics purchases,
            bool haveSelectedHelper = false,
            PlayerHelperTypes selectedHelper = PlayerHelperTypes.MachineGun)
        {
            Dictionary<GoodNames, List<(object value, int price)>> goodsContent = _goods.GetFormattedContent();
            List<(GoodNames good, object value)> viewContent = goodsContent
                .Select(item => (item.Key, item.Value[(int)ValueConstants.Zero].value))
                .ToList();
            ExpandPurchases((Purchases)purchases);

            _factory = new CardFactory(
                ConvertTemplates(),
                _goods.GetIcons(),
                _holder,
                GetSelected(selectedHelper),
                haveSelectedHelper);
            _model = new Shop(goodsContent, (Purchases)purchases, purchaseChangeCallback);
            _view = new ShopView(viewContent, _factory, selectionCallback);
            _presenter = new ShopPresenter(wallet, _model, _view);

            _presenter.Enable();
        }

        private Dictionary<Type, GoodCard> ConvertTemplates()
        {
            return _cardTemplates.ToDictionary(
                item => item.GetType(),
                item => item);
        }

        private GoodNames GetSelected(PlayerHelperTypes helperType)
        {
            return helperType switch
            {
                PlayerHelperTypes.MachineGun => GoodNames.MachineGun,
                PlayerHelperTypes.Grenade => GoodNames.Grenade,
                _ => throw new ArgumentOutOfRangeException(nameof(helperType), helperType, null)
            };
        }

        private void ExpandPurchases(Purchases purchases)
        {
            //*******************************
            Debug.Log("Before expand");
            purchases.Objects.ForEach(o => { Debug.Log($"{o.Key} {o.Value}");});
            //*******************************
            IEnumerable<SerializedPair<GoodNames, int>> content = Enum.GetValues(typeof(GoodNames))
                .Cast<GoodNames>()
                .Select(type => new SerializedPair<GoodNames, int>(type, (int)ValueConstants.Zero));

            foreach (SerializedPair<GoodNames, int> pair in content)
            {
                if (purchases.Objects.Exists(item => Equals(item.Key, pair.Key)) == false)
                    continue;

                purchases.Objects.Add(pair);
            }

            //*******************************
            Debug.Log("After expand");
            purchases.Objects.ForEach(o => { Debug.Log($"{o.Key} {o.Value}"); });
            //*******************************
        }
    }
}