using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class VictoryEffect : MonoBehaviour
    {
        [SerializeField] private ValueChangeEffect _changeEffect;
        [SerializeField] private TextMeshProUGUI _killsCountLabel;
        [SerializeField] private TextMeshProUGUI _currencyCountLabel;
        [SerializeField] private float _duration;
        [SerializeField] private AudioSource _moneySound;

        public async void PlayEffect(int kills, int currency)
        {
            if (kills <= 0)
                throw new ArgumentOutOfRangeException(nameof(kills));

            if (currency <= 0)
                throw new ArgumentOutOfRangeException(nameof(currency));

            _changeEffect.ChangeValue(kills, _duration, _killsCountLabel);
            int delay = (int)(_duration * 1000);
            await UniTask.Delay(delay);
            _moneySound.Play();
            _changeEffect.ChangeValue(currency, _duration, _currencyCountLabel);
        }
    }
}