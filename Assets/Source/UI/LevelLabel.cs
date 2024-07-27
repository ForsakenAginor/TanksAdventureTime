using Lean.Localization;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Source.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(LeanLocalizedTextMeshProUGUI))]
    public class LevelLabel : MonoBehaviour
    {
        private TextMeshProUGUI _label;
        private LeanLocalizedTextMeshProUGUI _localizationComponent;

        private void Awake()
        {
            _label = GetComponent<TextMeshProUGUI>();
            _localizationComponent = GetComponent<LeanLocalizedTextMeshProUGUI>();
        }

        public void Init(int level)
        {
            const string Translation = "Level";

            if (level <= 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            _localizationComponent.TranslationName = Translation;
            _localizationComponent.UpdateLocalization();
            _label.text += $" {level}";
        }
    }
}