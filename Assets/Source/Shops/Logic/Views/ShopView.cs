using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops
{
    public class ShopView : IDisposable
    {
        private const string Exception = "This type of object is not supported";

        private readonly List<ISelectable> _selectables = new ();
        private readonly List<ICard> _cards = new ();
        private readonly ICardFactory _factory;
        private readonly Action<int> _selectionCallback;

        public ShopView(
            List<(GoodNames good, object value)> goodsContent,
            ICardFactory factory,
            Action<int> selectionCallback)
        {
            _factory = factory;
            _selectionCallback = selectionCallback;
            CreateCards(goodsContent);
        }

        public event Action<ICard> GoingBuy;

        public event Action<List<ICard>> LoadRequested;

        public void Load()
        {
            LoadRequested?.Invoke(_cards);
        }

        public void Dispose()
        {
            foreach (ICard card in _cards)
            {
                card.Clicked -= CardClicked;

                if (card is IDisposable disposable == false)
                    continue;

                disposable.Dispose();
            }
        }

        public void Select(ISelectable target)
        {
            foreach (ISelectable selectable in _selectables)
                selectable.Deselect();

            target.Select();
            _selectionCallback?.Invoke((int)((ICard)target).Good);
        }

        private void CreateCards(List<(GoodNames good, object value)> goodsContent)
        {
            foreach (ICard card in goodsContent.Select(item => CreateCard(item.good, item.value)))
            {
                _cards.Add(card);
                card.Clicked += CardClicked;

                if (card is ISelectable == false)
                    continue;

                _selectables.Add((ISelectable)card);
            }
        }

        private ICard CreateCard(GoodNames good, object value)
        {
            return value switch
            {
                float => _factory.CreateNumber(good),
                int => _factory.CreateNumber(good),
                bool => _factory.CreateBool(good),
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, Exception)
            };
        }

        private void CardClicked(ICard card)
        {
            GoingBuy?.Invoke(card);
        }
    }
}