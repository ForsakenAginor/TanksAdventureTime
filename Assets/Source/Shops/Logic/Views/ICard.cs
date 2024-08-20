using System;

namespace Shops
{
    public interface ICard
    {
        public event Action<ICard> Clicked;

        public GoodNames Good { get; }

        public void ShowFailure();

        public void ShowMaximum();

        public void ShowNext((object currentValue, object nextValue, int price) purchase);
    }
}