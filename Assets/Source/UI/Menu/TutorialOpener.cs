using Agava.WebUtility;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorialOpener : MonoBehaviour
    {
        [SerializeField] private UserInterfaceElement _pcPanel;
        [SerializeField] private UserInterfaceElement _mobilePanel;
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

            if (Device.IsMobile)
                _mobilePanel.Enable();
            else
                _pcPanel.Enable();
        }
    }
}
