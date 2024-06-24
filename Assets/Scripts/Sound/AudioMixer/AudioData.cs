using System;
using UnityEngine;

namespace Assets.Scripts.Sound.AudioMixer
{
    public class AudioData
    {
        private const string MasterVolumeVariableName = "MasterVolumeValue";
        private const string EffectsVolumeVariableName = "EffectVolumeValue";
        private const string MusicVolumeVariableName = "MusicVolumeValue";
        private const float MinimumVolume = 0f;
        private const float MaximumVolume = 1f;

        public float GetMasterVolume()
        {
            if (PlayerPrefs.HasKey(MasterVolumeVariableName))
                return PlayerPrefs.GetFloat(MasterVolumeVariableName);
            else
                return MaximumVolume;
        }

        public void SaveMasterVolume(float value)
        {
            if (value < MinimumVolume || value > MaximumVolume)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetFloat(MasterVolumeVariableName, value);
            PlayerPrefs.Save();
        }

        public float GetEffectsVolume()
        {
            if (PlayerPrefs.HasKey(EffectsVolumeVariableName))
                return PlayerPrefs.GetFloat(EffectsVolumeVariableName);
            else
                return MaximumVolume;
        }

        public void SaveEffectsVolume(float value)
        {
            if (value < MinimumVolume || value > MaximumVolume)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetFloat(EffectsVolumeVariableName, value);
            PlayerPrefs.Save();
        }

        public float GetMusicVolume()
        {
            if (PlayerPrefs.HasKey(MusicVolumeVariableName))
                return PlayerPrefs.GetFloat(MusicVolumeVariableName);
            else
                return MaximumVolume;
        }

        public void SaveMusicVolume(float value)
        {
            if (value < MinimumVolume || value > MaximumVolume)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetFloat(MusicVolumeVariableName, value);
            PlayerPrefs.Save();
        }
    }
}