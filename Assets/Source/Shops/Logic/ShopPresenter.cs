using System;

namespace Shops
{
    public class ShopPresenter
    {
        private readonly Wallet _wallet;
        private readonly Shop _shop;
        private readonly ShopView _shopView;

        public ShopPresenter(Wallet wallet, Shop shop, ShopView shopView)
        {
            _wallet = wallet;
            _shop = shop;
            _shopView = shopView;
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        public void OnGoingBuy(ICard card)
        {
            if (_shop.TryGetPrice(card.Good, out int price) == false)
                throw new IndexOutOfRangeException();

            if (_wallet.TrySpentCurrency(price) == false)
            {
                card.ShowFailure();
                return;
            }

            _shop.Buy(card.Good);

            if (_shop.TryGetNextPurchase(card.Good, out var purchase) == false)
            {
                card.ShowMaximum();
                return;
            }

            card.ShowNext(purchase);
        }
    }
}