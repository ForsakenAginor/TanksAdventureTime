using System;

namespace Shops
{
    public class CurrencyCalculator
    {
        private readonly int _bounty;
        private readonly Wallet _wallet;

        public CurrencyCalculator(int bounty, Wallet wallet)
        {
            _bounty = bounty > 0 ? bounty : throw new ArgumentOutOfRangeException(nameof(bounty));
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        }

        public int CalculateTotalBounty(int kills)
        {
            if (kills <= 0)
                throw new ArgumentOutOfRangeException(nameof(kills));

            int totalBounty = kills * _bounty;
            _wallet.AddCurrency(totalBounty);
            return totalBounty;
        }
    }
}