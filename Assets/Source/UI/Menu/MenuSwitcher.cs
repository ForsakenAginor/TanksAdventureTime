using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UI.Menu
{
    public class MenuSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _targetPanel;
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
            _targetPanel.SetActive(true);
        }
    }
}