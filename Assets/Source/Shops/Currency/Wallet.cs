using System;

namespace Shops
{
    public class Wallet : IWallet
    {
        private CurrencyData _currencyData;
        private int _currentCurrency;

        public Wallet(CurrencyData currencyData)
        {
            _currencyData = currencyData != null ? currencyData : throw new ArgumentNullException(nameof(currencyData));
            _currentCurrency = _currencyData.GetCurrency();
        }

        public void AddCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currentCurrency += amount;
            _currencyData.Save(_currentCurrency);
        }

        public bool TrySpentCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (amount > _currentCurrency)
                return false;

            _currentCurrency -= amount;
            _currencyData.Save(_currentCurrency);
            return true;
        }
    }
}