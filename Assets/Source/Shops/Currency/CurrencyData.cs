using System;
using SavingData;

namespace Shops
{
    public class CurrencyData : ISave
    {
        public int GetCurrency()
        {
            return 0;
        }

        public void SaveCurrencyData(int currency)
        {
            if (currency < 0)
                throw new ArgumentOutOfRangeException(nameof(currency));
        }
    }
}