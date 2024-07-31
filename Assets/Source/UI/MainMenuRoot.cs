using Agava.YandexGames;
using Assets.Source.Sound.AudioMixer;
using UnityEngine;

namespace Assets.Source.UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private SoundInitializer _soundInitializer;

        private void Start()
        {
            _soundInitializer.Init();
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif
        }
    }
}