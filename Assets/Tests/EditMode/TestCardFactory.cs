using System;
using Shops;

namespace Tests.EditMode
{
    public class TestCardFactory : ICardFactory
    {
        private readonly Action<ICard> _onCardCreated;

        public TestCardFactory(Action<ICard> onCardCreated)
        {
            _onCardCreated = onCardCreated;
        }

        public ICard Create(NumberGoodCard target, GoodNames good)
        {
            return Create(good);
        }

        public ICard Create(BoolGoodCard target, GoodNames good)
        {
            return Create(good);
        }

        private ICard Create(GoodNames good)
        {
            ICard card = new TestCard(good);
            _onCardCreated?.Invoke(card);
            return card;
        }
    }
}