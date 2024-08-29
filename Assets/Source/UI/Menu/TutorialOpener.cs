using Agava.WebUtility;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorialOpener : MonoBehaviour
    {
        [SerializeField] private GameObject _pcPanel;
        [SerializeField] private GameObject _mobilePanel;
        [SerializeField] private GameObject _holderPanel;
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
            _holderPanel.SetActive(false);

            if (Device.IsMobile)
                _mobilePanel.SetActive(true);
            else
                _pcPanel.SetActive(true);
        }
    }
}
