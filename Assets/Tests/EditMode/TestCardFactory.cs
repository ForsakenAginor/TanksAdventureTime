using System;
using Shops;

public class TestCardFactory : ICardFactory
{
    private readonly Action<ICard> _onCardCreated;

    public TestCardFactory(Action<ICard> onCardCreated)
    {
        _onCardCreated = onCardCreated;
    }

    public ICard CreateNumber(GoodNames good)
    {
        return Create(good);
    }

    public ICard CreateBool(GoodNames good)
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