using TMPro;
using UnityEngine;

namespace Player.HealthSystem
{
    public class TextHealthView : HealthView
    {
        [SerializeField] private TextMeshProUGUI _label;

        protected override void OnHealthChanged(int current, int max)
        {
            _label.text = $"{current}/{max}";
        }
    }
}