namespace Shops
{
    public interface ICardFactory
    {
        public ICard CreateNumber(GoodNames good);

        public ICard CreateBool(GoodNames good);
    }
}