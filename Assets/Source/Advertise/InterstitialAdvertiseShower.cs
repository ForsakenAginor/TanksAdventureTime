#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using Agava.YandexGames;
using UnityEngine;

namespace Advertise
{
    public class InterstitialAdvertiseShower
    {
        private readonly Silencer _silencer;

        public InterstitialAdvertiseShower(Silencer silencer)
        {
            _silencer = silencer != null ? silencer : throw new ArgumentNullException(nameof(silencer));
        }

        public void ShowAdvertise()
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            AudioListener.volume = 0f;
            InterstitialAd.Show(null, OnCloseAdvertise);
        }

        private void OnCloseAdvertise(bool nonMatterVariable)
        {
            AudioListener.pause = false;
            AudioListener.volume = 1f;
            Time.timeScale = 0f;
            _silencer.SetGameState(Time.timeScale, AudioListener.volume, AudioListener.pause);
        }
    }
}
#endif