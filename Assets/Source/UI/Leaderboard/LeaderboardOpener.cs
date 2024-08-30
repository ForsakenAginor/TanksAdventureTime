using Agava.YandexGames;
using Difficulty;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderboardOpener : MonoBehaviour
    {
        [SerializeField] private YandexLeaderboard _leaderboard;
        [SerializeField] private GameObject _leaderboardPanel;
        [SerializeField] private Button _openLeaderboardButton;
        [SerializeField] private GameObject _holderPanel;
        [SerializeField] private GameObject _autorizationPanel;

        private void OnEnable()
        {
            _openLeaderboardButton.onClick.AddListener(OpenLeaderboard);
        }

        private void OnDisable()
        {
            _openLeaderboardButton.onClick.RemoveListener(OpenLeaderboard);
        }

        public void OpenLeaderboard()
        {
            bool isAuthorized;

#if UNITY_EDITOR
            isAuthorized = false;
#else
        isAuthorized = PlayerAccount.IsAuthorized;
#endif

            if (isAuthorized)
                PlayerAccount.RequestPersonalProfileDataPermission(OnSuccessCallback, OnErrorCallback);
            else
                _autorizationPanel.SetActive(true);

            _holderPanel.SetActive(false);
        }

#if UNITY_EDITOR
        public void ShowPanel()
        {
            _leaderboardPanel.SetActive(true);
        }
#endif

        private void OnErrorCallback(string nonmatterValue)
        {
            Fill();
        }

        private void OnSuccessCallback()
        {
            Fill();
        }

        private void Fill()
        {
            _leaderboard.Fill();
            _leaderboardPanel.SetActive(true);
        }
    }
}