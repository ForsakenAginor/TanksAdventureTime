using System.Collections;
using Agava.YandexGames;
using Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EntryPoint
{
    public class SDKInitializer : MonoBehaviour
    {
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
            string language = "ru";

#if UNITY_WEBGL && !UNITY_EDITOR
            while (YandexGamesSdk.IsInitialized == false)
                yield return YandexGamesSdk.Initialize();

            language = YandexGamesSdk.Environment.i18n.lang;
#else
            yield return null;
#endif

            LocalizationInitializer localizationInitializer = new ();
            localizationInitializer.ApplyLocalization(language);
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}