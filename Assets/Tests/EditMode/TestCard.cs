using Shops;

public class TestCard : ICard
{
    public TestCard(GoodNames good)
    {
        Good = good;
    }

    public GoodNames Good { get; }

    public void ShowFailure()
    {
    }

    public void ShowMaximum()
    {
    }

    public void ShowNext((object currentValue, object nextValue, int price) purchase)
    {
    }
}