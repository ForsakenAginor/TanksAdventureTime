using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SoundSettingOpener : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _soundSettingsHolder;
        [SerializeField] private GameObject _buttonCanvas;

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenSettings);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenSettings);
        }

        private void OpenSettings()
        {
            Time.timeScale = 0f;
            _soundSettingsHolder.SetActive(true);
            _buttonCanvas.SetActive(false);
        }
    }
}