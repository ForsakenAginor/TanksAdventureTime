using System;
using TMPro;

namespace Shops
{
    public class WalletView
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private Wallet _wallet;

        public WalletView(Wallet wallet, TextMeshProUGUI textField)
        {
            _wallet = wallet != null ? wallet : throw new ArgumentNullException(nameof(wallet));
            _textMeshProUGUI = textField != null ? textField : throw new ArgumentNullException(nameof(textField));
            _textMeshProUGUI.text = _wallet.CurrentCurrency.ToString();
            _wallet.CurrencyAmountChanged += OnCurrencyAmountChanged;
        }

        ~WalletView() => _wallet.CurrencyAmountChanged -= OnCurrencyAmountChanged;

        private void OnCurrencyAmountChanged(int currency) => _textMeshProUGUI.text = currency.ToString();
    }
}