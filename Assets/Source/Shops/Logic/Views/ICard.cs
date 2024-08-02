namespace Shops
{
    public interface ICard
    {
        public GoodNames Good { get; }

        public void ShowFailure();

        public void ShowMaximum();

        public void ShowNext((object currentValue, object nextValue, int price) purchase);
    }
}