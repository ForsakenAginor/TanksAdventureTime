using System.Collections;
using Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EntryPoint
{
    public class SDKInitializer : MonoBehaviour
    {
        private void Awake()
        {
            //YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
            string language = "en";
            yield return null;

            LocalizationInitializer localizationInitializer = new();
            localizationInitializer.ApplyLocalization(language);
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}