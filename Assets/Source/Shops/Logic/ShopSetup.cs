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
            Action<GoodNames> selectionCallback = null,
            Purchases<int> purchases = null,
            bool haveSelectedHelper = false,
            PlayerHelperTypes selectedHelper = PlayerHelperTypes.MachineGun)
        {
            Dictionary<GoodNames, List<(object value, int price)>> goodsContent = _goods.GetContent();
            List<(GoodNames good, object value)> viewContent =
                goodsContent.Select(item => (item.Key, item.Value[(int)ValueConstants.Zero].value)).ToList();
            purchases ??= CreateStartPurchases();

            _factory = new CardFactory(
                ConvertTemplates(),
                _goods.GetIcons(),
                _holder,
                GetSelected(selectedHelper),
                haveSelectedHelper);
            _model = new Shop(goodsContent, purchases);
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

        private Purchases<int> CreateStartPurchases()
        {
            List<SerializedPair<GoodNames, int>> values = Enum.GetValues(typeof(GoodNames))
                .Cast<GoodNames>()
                .Select(type => new SerializedPair<GoodNames, int>(type, (int)ValueConstants.Zero)).ToList();

            return new Purchases<int>(values);
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
    }
}