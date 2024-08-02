using System;
using Shops;
using UnityEngine;

public class TestCard : ICard
{
    public TestCard(GoodNames good)
    {
        Good = good;
    }

    public event Action<ICard> Clicked;

    public GoodNames Good { get; }

    public bool DidFailed { get; private set; }

    public bool IsMaximum { get; private set; }

    public void Click()
    {
        Clicked?.Invoke(this);
    }

    public void ShowFailure()
    {
        DidFailed = true;
    }

    public void ShowMaximum()
    {
        IsMaximum = true;
    }

    public void ShowNext((object currentValue, object nextValue, int price) purchase)
    {
    }
}