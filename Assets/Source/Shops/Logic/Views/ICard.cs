using System;

namespace Shops
{
    public interface ICard
    {
        public event Action<ICard> Clicked;

        public GoodNames Good { get; }

        public void ShowFailure();

        public void ShowMaximum(object currentValue = null);

        public void ShowNext((object currentValue, object nextValue, int price) purchase);
    }
}