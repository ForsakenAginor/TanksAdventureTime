using Lean.Localization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.Leaderboard
{
    [RequireComponent(typeof(LeanLocalizedTextMeshProUGUI))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnonymousLocalizator : MonoBehaviour
    {
        private LeanLocalizedTextMeshProUGUI _localizedText;
        private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            _localizedText = GetComponent<LeanLocalizedTextMeshProUGUI>();
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            const string AnonymousName = "Anonymous";

            if (_textMeshProUGUI.text == AnonymousName)
            {
                _localizedText.TranslationName = AnonymousName;
                _localizedText.UpdateLocalization();
            }
        }
    }
}