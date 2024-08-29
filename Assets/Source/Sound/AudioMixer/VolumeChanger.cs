using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public class VolumeChanger
    {
        private const string Warning = " is already added to VolumeChanger";

        private readonly List<AudioSource> _audioSources;

        private float _previousVolumeValue;

        public VolumeChanger(List<AudioSource> audioSources, float volumeValue = 1f)
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

        public void AddAudioSource(AudioSource audioSource)
        {
            if (audioSource == null)
                throw new ArgumentNullException(nameof(audioSource));

            if (_audioSources.Contains(audioSource))
                throw new Exception(audioSource + Warning);

            audioSource.volume *= _previousVolumeValue;
            _audioSources.Add(audioSource);
        }
    }
}