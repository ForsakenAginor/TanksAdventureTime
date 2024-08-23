using System;

namespace Shops
{
    public class CurrencyData : ISave
    {
        public int GetCurrency()
        {
            return 0;
        }

        public void SetCurrencyData(int currency)
        {
            if (currency < 0)
                throw new ArgumentOutOfRangeException(nameof(currency));
        }
    }
}