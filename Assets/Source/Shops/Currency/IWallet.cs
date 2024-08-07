namespace Shops
{
    public interface IWallet
    {
        public bool TrySpentCurrency(int amount);
    }
}