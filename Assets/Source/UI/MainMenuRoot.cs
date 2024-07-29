using Agava.YandexGames;
using Assets.Source.Sound.AudioMixer;
using UnityEngine;

namespace Assets.Source.UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private SoundInitializer _soundInitializer;
        [SerializeField] private SaveService _saveService;

        private void Start()
        {
            // _soundInitializer.Init();
            Debug.Log("Start");
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif
        }

        //
        private void OnEnable()
        {
            _saveService.SaveGameData.Loaded += InitSound;
        }

        private void OnDisable()
        {
            _saveService.SaveGameData.Loaded -= InitSound;
        }

        private void InitSound()
        {
            Debug.Log("ED");
            _soundInitializer.Init();
        }
        //
    }
}