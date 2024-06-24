using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sound.AudioMixer
{
    public class VolumeSaver : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SoundInitializer _soundInitializer;

        private void OnEnable()
        {
            _button.onClick.AddListener(Save);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Save);
        }

        private void Save()
        {
            _soundInitializer.SaveSettings();
        }
    }
}