#if UNITY_WEBGL && !UNITY_EDITOR
using Agava.YandexGames;
#endif
using Assets.Source.Sound.AudioMixer;
using Shops;
using UnityEngine;

namespace Assets.Source.UI
{
    public class MainMenuRoot : MonoBehaviour
    {
        [SerializeField] private SoundInitializer _soundInitializer;
        [SerializeField] private ShopSetup _shop;
        [SerializeField] private SaveService _saveService;

        private void Start()
        {
            _soundInitializer.Init();
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif
            _shop.Init(
                new Wallet(_saveService),
                _saveService.SavePurchasesData,
                _saveService.SetPlayerHelperData,
                _saveService.GetPurchasesData());
        }
    }
}