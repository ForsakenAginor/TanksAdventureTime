using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuSwitcher : MonoBehaviour
    {
        [SerializeField] private UserInterfaceElement _targetPanel;
        [SerializeField] private UserInterfaceElement _holderPanel;
        [SerializeField] private Button _toggleButton;

        private void OnEnable()
        {
            _toggleButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _toggleButton.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _holderPanel.Disable();
            _targetPanel.Enable();
        }
    }
}