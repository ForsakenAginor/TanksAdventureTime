using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderboardOpener : MonoBehaviour
    {
        [SerializeField] private YandexLeaderboard _leaderboard;
        [SerializeField] private UserInterfaceElement _leaderboardPanel;
        [SerializeField] private Button _openLeaderboardButton;
        [SerializeField] private UserInterfaceElement _holderPanel;
        [SerializeField] private UserInterfaceElement _autorizationPanel;

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
                _autorizationPanel.Enable();

            _holderPanel.Disable();
        }

#if UNITY_EDITOR
        public void ShowPanel()
        {
            _leaderboardPanel.Enable();
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
            _leaderboardPanel.Enable();
        }
    }
}