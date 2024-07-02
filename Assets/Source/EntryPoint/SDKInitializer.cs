using System.Collections;
using Agava.YandexGames;
using Assets.Source.Global.Enums;
using Assets.Source.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source.EntryPoint
{
    public class SDKInitializer : MonoBehaviour
    {
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
            string language;

#if UNITY_WEBGL && !UNITY_EDITOR
            while (YandexGamesSdk.IsInitialized == false)
                yield return YandexGamesSdk.Initialize();
            
            language = YandexGamesSdk.Environment.i18n.lang;
#else
            yield return null;
            language = "ru";
#endif

            LocalizationInitializer localizationInitializer = new();
            localizationInitializer.ApplyLocalization(language);
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}