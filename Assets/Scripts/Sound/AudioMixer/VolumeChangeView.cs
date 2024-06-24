using System;
using UnityEngine.UI;

namespace Assets.Scripts.Sound.AudioMixer
{
    public class VolumeChangeView
    {
        private readonly Slider _slider;
        private readonly VolumeChanger _volumeChanger;

        public VolumeChangeView(VolumeChanger volumeChanger, Slider slider)
        {
            _volumeChanger = volumeChanger != null ? volumeChanger : throw new ArgumentNullException(nameof(volumeChanger));
            _slider = slider != null ? slider : throw new ArgumentNullException(nameof(slider));
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        ~VolumeChangeView()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _volumeChanger.ChangeVolume(value);
        }
    }
}