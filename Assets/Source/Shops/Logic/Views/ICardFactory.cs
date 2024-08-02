namespace Shops
{
    public interface ICardFactory
    {
        public ICard Create(NumberGoodCard target, GoodNames good);

        public ICard Create(BoolGoodCard target, GoodNames good);
    }
}