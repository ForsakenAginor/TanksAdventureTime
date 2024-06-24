using System;
using UnityEngine;

namespace Assets.Scripts.Sound.AudioMixer
{
    public class VolumeChanger
    {
        private readonly AudioSource[] _audioSources;
        private float _previousVolumeValue;

        public VolumeChanger(AudioSource[] audioSources, float volumeValue = 1f)
        {
            _audioSources = audioSources != null ? audioSources : throw new ArgumentNullException(nameof(audioSources));

            if (volumeValue < 0f || volumeValue > 1f)
                throw new ArgumentOutOfRangeException(nameof(volumeValue));

            foreach (var source in _audioSources)
                source.volume *= volumeValue;

            _previousVolumeValue = volumeValue;
        }

        public void ChangeVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);

            foreach (var source in _audioSources)
                source.volume = source.volume / _previousVolumeValue * volume;

            _previousVolumeValue = volume;
        }
    }
}