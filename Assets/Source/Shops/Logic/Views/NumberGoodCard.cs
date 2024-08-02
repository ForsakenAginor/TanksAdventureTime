using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shops
{
    public class NumberGoodCard : GoodCard
    {
        private const string Max = nameof(Max);

        [SerializeField] private TextMeshProUGUI _currentGrade;
        [SerializeField] private TextMeshProUGUI _nextGrade;
        [SerializeField] private List<GameObject> _hideObjects;

        public override void ShowMaximum()
        {
            ChangeButtonTranslation(Max);
            HidePrice(HideGrade);
        }

        public override void ShowNext((object currentValue, object nextValue, int price) purchase)
        {
            SetPrice(purchase.price);
            _currentGrade.SetText(FormatValue(purchase.currentValue));
            _nextGrade.SetText(FormatValue(purchase.nextValue));
        }

        private string FormatValue(object value)
        {
            return value is int == true ? $"{value}" : $"{value:F1}";
        }

        private void HideGrade()
        {
            foreach (GameObject hideObject in _hideObjects)
                hideObject.SetActive(false);
        }
    }
}