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
            while (YandexGamesSdk.IsInitialized == false)
                yield return YandexGamesSdk.Initialize();

            string language = YandexGamesSdk.Environment.i18n.lang;

            LocalizationInitializer localizationInitializer = new ();
            localizationInitializer.ApplyLocalization(language);
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}