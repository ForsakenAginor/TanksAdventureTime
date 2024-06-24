using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class MenuSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _targetPanel;
        [SerializeField] private GameObject _holderPanel;
        [SerializeField] private Button _toggleButton;

        private void OnEnable()
        {
            _toggleButton.onClick.AddListener(ShowTargetPanel);
        }

        private void OnDisable()
        {
            _toggleButton.onClick.RemoveListener(ShowTargetPanel);
        }

        private void ShowTargetPanel()
        {
            _holderPanel.SetActive(false);
            _targetPanel.SetActive(true);
        }
    }
}