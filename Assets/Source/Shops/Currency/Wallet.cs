using System;
using SavingData;

namespace Shops
{
    public class Wallet : IWallet
    {
        private readonly ISave _save;

        public Wallet(ISave save)
        {
            _save = save ?? throw new ArgumentNullException(nameof(save));
            CurrentCurrency = _save.GetCurrency();
            _save = save;
        }

        public event Action<int> CurrencyAmountChanged;

        public int CurrentCurrency { get; private set; }

        public void AddCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            CurrentCurrency += amount;
            _save.SaveCurrencyData(CurrentCurrency);
            CurrencyAmountChanged?.Invoke(CurrentCurrency);
        }

        public bool TrySpentCurrency(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            if (amount > CurrentCurrency)
                return false;

            CurrentCurrency -= amount;
            _save.SaveCurrencyData(CurrentCurrency);
            CurrencyAmountChanged?.Invoke(CurrentCurrency);
            return true;
        }
    }
}