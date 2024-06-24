using System.Collections;
using Agava.YandexGames;
using Assets.Scripts.Core;
using Assets.Scripts.UI.Menu.LevelSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
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
            language = "en";
#endif

            LocalizationInitializer localizationInitializer = new ();
            localizationInitializer.ApplyLocalization(language);
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}