using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Sound.AudioMixer
{
    public class SoundInitializer : MonoBehaviour
    {
        private readonly AudioData _audioData = new();

        [Header("AudioSources")]
        [SerializeField] private List<AudioSource> _allSources;
        [SerializeField] private List<AudioSource> _effectsSources;
        [SerializeField] private List<AudioSource> _musicSources;

        [Header("Sliders")]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        [Header("VolumeChangers")]
        private VolumeChanger _masterChanger;
        private VolumeChanger _effectsChanger;
        private VolumeChanger _musicChanger;

        public void Init()
        {
            _masterVolumeSlider.value = _audioData.GetMasterVolume();
            _effectsVolumeSlider.value = _audioData.GetEffectsVolume();
            _musicVolumeSlider.value = _audioData.GetMusicVolume();

            _masterChanger = new(_allSources, _audioData.GetMasterVolume());
            _effectsChanger = new(_effectsSources, _audioData.GetEffectsVolume());
            _musicChanger = new(_musicSources, _audioData.GetMusicVolume());
            VolumeChangeView masterChangerView = new(_masterChanger, _masterVolumeSlider);
            VolumeChangeView effectsChangerView = new(_effectsChanger, _effectsVolumeSlider);
            VolumeChangeView musicChangerView = new(_musicChanger, _musicVolumeSlider);
        }

        public void SaveSettings()
        {
            _audioData.SaveMasterVolume(_masterVolumeSlider.value);
            _audioData.SaveEffectsVolume(_effectsVolumeSlider.value);
            _audioData.SaveMusicVolume(_musicVolumeSlider.value);
        }

        public void AddMusicSource(AudioSource music)
        {
            if (music == null)
                throw new ArgumentNullException(nameof(music));

            _masterChanger.AddAudioSource(music);
            _musicChanger.AddAudioSource(music);
        }

        public void AddEffectSource(AudioSource effect)
        {
            if (effect == null)
                throw new ArgumentNullException(nameof(effect));

            _masterChanger.AddAudioSource(effect);
            _effectsChanger.AddAudioSource(effect);
        }
    }
}