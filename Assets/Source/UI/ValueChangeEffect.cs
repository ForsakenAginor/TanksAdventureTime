using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ValueChangeEffect : MonoBehaviour
    {
        public void ChangeValue(int endValue, float duration, TextMeshProUGUI text)
        {
            if (duration <= 0)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (endValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(endValue));

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            StartCoroutine(StartCalculate(endValue, duration, text));
        }

        private IEnumerator StartCalculate(int endValue, float duration, TextMeshProUGUI text)
        {
            float step = 0.1f;
            float additiveValue = endValue / duration * step;
            float previousValue = 0;
            WaitForSeconds delay = new (step);

            text.text = $"{previousValue}";

            for (float i = previousValue; i <= endValue; i += additiveValue)
            {
                yield return delay;
                previousValue = UnityEngine.Random.Range(previousValue, i);
                text.text = $"{previousValue:0}";
            }

            text.text = $"{endValue}";
        }
    }
}