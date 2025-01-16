using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationChanger : MonoBehaviour
{
    [Header("Localization")]
    private readonly string _russian = "Russian";
    private readonly string _english = "English";
    private readonly string _turkish = "Turkish";
    [SerializeField] private Button _toEnglish;
    [SerializeField] private Button _toTurkish;
    [SerializeField] private Button _toRussian;

    private void Awake()
    {
        _toEnglish.onClick.AddListener(ChangeLanguageToEnglish);
        _toTurkish.onClick.AddListener(ChangeLanguageToTurkish);
        _toRussian.onClick.AddListener(ChangeLanguageToRussian);
    }

    private void OnDestroy()
    {
        _toEnglish.onClick.RemoveListener(ChangeLanguageToEnglish);
        _toTurkish.onClick.RemoveListener(ChangeLanguageToTurkish);
        _toRussian.onClick.RemoveListener(ChangeLanguageToRussian);
    }

    private void ChangeLanguageToRussian()
    {
        SetLanguage(_russian);
    }

    private void ChangeLanguageToTurkish()
    {
        SetLanguage(_turkish);
    }

    private void ChangeLanguageToEnglish()
    {
        SetLanguage(_english);
    }

    private void SetLanguage(string language)
    {
        LeanLocalization.SetCurrentLanguageAll(language);
        LeanLocalization.UpdateTranslations();
    }
}