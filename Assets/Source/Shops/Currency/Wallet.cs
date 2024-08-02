using System;

namespace Shops
{
    public class Wallet : IWallet
    {
        private ISave _save;
        private int _currentCurrency;

        public int CurrentCurrency => _currentCurrency;
        public Wallet(ISave save)
        {
            _save = save != null ? save : throw new ArgumentNullException(nameof(save));
            _currentCurrency = _save.GetCurrency();
            _save = save;
        }

        public void AddCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currentCurrency += amount;
            _save.Save(_currentCurrency);
        }

        public bool TrySpentCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (amount > _currentCurrency)
                return false;

            _currentCurrency -= amount;
            _save.Save(_currentCurrency);
            return true;
        }
    }
}