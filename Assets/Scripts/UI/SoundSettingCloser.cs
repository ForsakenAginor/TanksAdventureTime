using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SoundSettingCloser : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _soundSettingsHolder;
        [SerializeField] private GameObject _buttonsCanvas;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Time.timeScale = 1f;
            _soundSettingsHolder.SetActive(false);
            _buttonsCanvas.SetActive(true);
        }
    }
}