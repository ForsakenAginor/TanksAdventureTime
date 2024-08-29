using System;
using TMPro;

namespace Shops
{
    public class WalletView
    {
        private const string Symbol = "x ";

        private readonly TextMeshProUGUI _textMeshProUGUI;
        private readonly Wallet _wallet;

        public WalletView(Wallet wallet, TextMeshProUGUI textField)
        {
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            _textMeshProUGUI = textField != null ? textField : throw new ArgumentNullException(nameof(textField));
            OnCurrencyAmountChanged(_wallet.CurrentCurrency);
            _wallet.CurrencyAmountChanged += OnCurrencyAmountChanged;
        }

        ~WalletView() => _wallet.CurrencyAmountChanged -= OnCurrencyAmountChanged;

        private void OnCurrencyAmountChanged(int currency) => _textMeshProUGUI.text = $"{Symbol}{currency}";
    }
}