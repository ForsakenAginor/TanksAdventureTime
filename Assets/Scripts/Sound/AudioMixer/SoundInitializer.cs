using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sound.AudioMixer
{
    public class SoundInitializer : MonoBehaviour
    {
        private readonly AudioData _audioData = new ();

        [Header("AudioSources")]
        [SerializeField] private AudioSource[] _allSources;
        [SerializeField] private AudioSource[] _effectsSources;
        [SerializeField] private AudioSource[] _musicSources;

        [Header("Sliders")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        private void Start()
        {
            _masterVolumeSlider.value = _audioData.GetMasterVolume();
            _effectsVolumeSlider.value = _audioData.GetEffectsVolume();
            _musicVolumeSlider.value = _audioData.GetMusicVolume();

            VolumeChanger masterChanger = new (_allSources, _audioData.GetMasterVolume());
            VolumeChanger effectsChanger = new (_effectsSources, _audioData.GetEffectsVolume());
            VolumeChanger musicChanger = new (_musicSources, _audioData.GetMusicVolume());
            VolumeChangeView masterChangerView = new (masterChanger, _masterVolumeSlider);
            VolumeChangeView effectsChangerView = new (effectsChanger, _effectsVolumeSlider);
            VolumeChangeView musicChangerView = new (musicChanger, _musicVolumeSlider);
        }

        public void SaveSettings()
        {
            _audioData.SaveMasterVolume(_masterVolumeSlider.value);
            _audioData.SaveEffectsVolume(_effectsVolumeSlider.value);
            _audioData.SaveMusicVolume(_musicVolumeSlider.value);
        }
    }
}