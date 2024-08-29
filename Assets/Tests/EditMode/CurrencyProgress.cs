using System;
using SavingProgress;

public class CurrencyProgress : ISave
{
    public int GetCurrency()
    {
        return 0;
    }

    public void SaveCurrency(int currency)
    {
        if (currency < 0)
            throw new ArgumentOutOfRangeException(nameof(currency));
    }
}