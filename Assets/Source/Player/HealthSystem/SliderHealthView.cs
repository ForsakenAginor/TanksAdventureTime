using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Player.HealthSystem
{
    public class SliderHealthView : HealthView
    {
        [SerializeField] private Slider _slider;

        protected override void OnHealthChanged(int current, int max)
        {
            _slider.maxValue = max;
            _slider.value = current;
        }
    }
}