using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class AuthorizationButtonHandler : MonoBehaviour
    {
        [SerializeField] private LeaderboardOpener _leaderboardOpener;
        [SerializeField] private UserInterfaceElement _holderPanel;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ShowLeaderboard);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ShowLeaderboard);
        }

        private void ShowLeaderboard()
        {
#if UNITY_EDITOR
            _leaderboardOpener.ShowPanel();
            _holderPanel.Disable();
#else
            PlayerAccount.Authorize(OnSuccessAuthorize);
#endif
        }

        private void OnSuccessAuthorize()
        {
            _leaderboardOpener.OpenLeaderboard();
            _holderPanel.Disable();
        }
    }
}