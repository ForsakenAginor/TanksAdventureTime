using Lean.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.Leaderboard
{
    internal class LeaderboardElement : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _nameField;
        [SerializeField] private TMPro.TextMeshProUGUI _rankField;
        [SerializeField] private TMPro.TextMeshProUGUI _scoreField;

        internal void Init(string name, int rank, int score)
        {
            const string AnonymousName = "Anonymous";

            _scoreField.text = score.ToString();
            _rankField.text = rank.ToString();

            if (name != AnonymousName)
            {
                _nameField.text = name;
                return;
            }

            LeanLocalizedTextMeshProUGUI localizedText =
                _nameField.gameObject.AddComponent(typeof(LeanLocalizedTextMeshProUGUI)) as LeanLocalizedTextMeshProUGUI;
            localizedText.TranslationName = AnonymousName;
            localizedText.UpdateLocalization();
        }
    }
}
