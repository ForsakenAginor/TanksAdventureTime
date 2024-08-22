using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shops
{
    public class ShopPresenter
    {
        private readonly IWallet _wallet;
        private readonly Shop _shop;
        private readonly ShopView _shopView;

        public ShopPresenter(IWallet wallet, Shop shop, ShopView shopView)
        {
            _wallet = wallet;
            _shop = shop;
            _shopView = shopView;
        }

        public void Enable()
        {
            _shopView.GoingBuy += OnGoingBuy;
            _shopView.LoadRequested += OnLoadRequested;

            _shopView.Load();
        }

        public void Disable()
        {
            _shopView.GoingBuy -= OnGoingBuy;
            _shopView.LoadRequested -= OnLoadRequested;

            _shopView.Dispose();
        }

        private void OnLoadRequested(List<ICard> cards)
        {
            foreach (ICard card in cards)
                Update(card);
        }

        private void OnGoingBuy(ICard card)
        {
            if (_shop.TryGetPrice(card.Good, out int price) == false)
            {
                if (TrySelect(card) == false)
                    throw new IndexOutOfRangeException(nameof(card));

                return;
            }

            if (_wallet.TrySpentCurrency(price) == false)
            {
                card.ShowFailure();
                return;
            }

            _shop.Buy(card.Good);
            Update(card, OnSelectableClick);
        }

        private void Update(ICard card, Action<ICard> selectionHandler = null)
        {
            if (_shop.TryGetNextPurchase(card.Good, out var purchase) == false)
            {
                card.ShowMaximum(_shop.GetCurrentValue(card.Good));
                selectionHandler?.Invoke(card);
                return;
            }

            card.ShowNext(purchase);
        }

        private void OnSelectableClick(ICard card)
        {
            TrySelect(card);
        }

        private bool TrySelect(ICard card)
        {
            if (card is ISelectable target == false)
                return false;

            _shopView.Select(target);
            return true;
        }
    }
}