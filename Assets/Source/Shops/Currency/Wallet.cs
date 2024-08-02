using System;

namespace Shops
{
    public class Wallet : IWallet
    {
        private SaveService _saveService;
        private int _currentCurrency;

        public int CurrentCurrency => _currentCurrency;
        public Wallet(SaveService saveService)
        {
            _saveService = saveService != null ? saveService : throw new ArgumentNullException(nameof(saveService));
            _currentCurrency = _saveService.Currency;
            _saveService = saveService;
        }

        public void AddCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currentCurrency += amount;
            _saveService.SetCurrencyData(_currentCurrency);
            _saveService.Save();
        }

        public bool TrySpentCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (amount > _currentCurrency)
                return false;

            _currentCurrency -= amount;
            _saveService.SetCurrencyData(_currentCurrency);
            _saveService.Save();
            return true;
        }
    }
}