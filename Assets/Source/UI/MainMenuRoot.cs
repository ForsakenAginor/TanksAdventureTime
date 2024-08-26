#if UNITY_WEBGL && !UNITY_EDITOR
using Agava.YandexGames;
#endif
using Agava.WebUtility;
using Assets.Source.SavingData;
using Assets.Source.Sound.AudioMixer;
using PlayerHelpers;
using Shops;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private GameObject _pcTutorialPanel;
        [SerializeField] private GameObject _mobileTutorialPanel;
        [SerializeField] private Button _endTutorialButton;
        [SerializeField] private SoundInitializer _soundInitializer;
        [SerializeField] private ShopSetup _shop;
        [SerializeField] private TextMeshProUGUI _currencyView;
        [SerializeField] private SaveService _saveService;

        private void OnEnable()
        {
            _saveService.Loaded += OnSaveLoaded;
            _endTutorialButton.onClick.AddListener(CompleteTutorial);
        }

        private void OnDisable()
        {
            _saveService.Loaded -= OnSaveLoaded;
            _endTutorialButton.onClick.RemoveListener(CompleteTutorial);
        }

        private void OnSaveLoaded()
        {
            const int CompletedTraining = 0;

            if (_saveService.CompletedTrainingOnComputer == CompletedTraining)
            {
                if (Device.IsMobile)
                    _mobileTutorialPanel.SetActive(true);
                else
                    _pcTutorialPanel.SetActive(true);
            }

            _soundInitializer.Init();
            Wallet wallet = new(_saveService);
            WalletView walletView = new(wallet, _currencyView);

            _shop.Init(
                wallet,
                _saveService.SavePurchasesData,
                _saveService.SetPlayerHelperData,
                _saveService.GetPurchasesData(),
                _saveService.HadHelper,
                (PlayerHelperTypes)_saveService.Helper);

#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif
        }

        private void CompleteTutorial()
        {
            _saveService.SetCompletedTrainingComputerData(true);
        }
    }
}