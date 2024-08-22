using System;
using UnityEngine;

namespace Shops
{
    public class CurrencyData : ISave
    {
        private const string CurrencyVariableName = "Currency";

        public int GetCurrency()
        {
            if (PlayerPrefs.HasKey(CurrencyVariableName) == false)
                return 0;

            return PlayerPrefs.GetInt(CurrencyVariableName);
        }

        public void SetCurrencyData(int currency)
        {
            if (currency < 0)
                throw new ArgumentOutOfRangeException(nameof(currency));

            PlayerPrefs.SetInt(CurrencyVariableName, currency);
            PlayerPrefs.Save();
        }
    }
}