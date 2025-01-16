using PlayerHelpers;
using SavingProgress;
using Shops;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private UserInterfaceElement _pcTutorialPanel;
        [SerializeField] private UserInterfaceElement _mobileTutorialPanel;
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
                bool isWebGLOnMobile = Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer;
                if (isWebGLOnMobile)
                    _mobileTutorialPanel.Enable();
                else
                    _pcTutorialPanel.Enable();
            }

            _soundInitializer.Init();
            Wallet wallet = new (_saveService);
            WalletView _ = new (wallet, _currencyView);

            _shop.Init(
                wallet,
                _saveService.SavePurchases,
                _saveService.SavePlayerHelper,
                _saveService.GetPurchases(),
                _saveService.HadHelper,
                (PlayerHelperTypes)_saveService.Helper);
        }

        private void CompleteTutorial()
        {
            _saveService.SaveCompletedTrainingComputer(true);
        }
    }
}