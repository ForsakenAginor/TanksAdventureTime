using Agava.WebUtility;
using Assets.Source.EntryPoint;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.Advertise
{
    [RequireComponent(typeof(Button))]
    public class ShowAdvertiseButtonHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _holder;
        [SerializeField] private GameObject _mobileInputCanvas;
        [SerializeField] private GameObject[] _objectsThatWillBeEnabled;
        [SerializeField] private Root _root;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(ShowVideoAd);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ShowVideoAd);
        }

        private void ShowVideoAd()
        {
#if !UNITY_EDITOR
        Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardCallback, OnCloseCallback);
#else
            OnRewardCallback();
#endif
        }

        private void OnOpenCallback()
        {
            _button.interactable = false;
            Time.timeScale = 0;
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }

        private void OnRewardCallback()
        {
            _holder.SetActive(false);
            _objectsThatWillBeEnabled.ToList().ForEach(o => o.SetActive(true));
            _root.Respawn();

#if !UNITY_EDITOR
        if (Device.IsMobile)
            _mobileInputCanvas.SetActive(true);
#else
            _mobileInputCanvas.SetActive(true);
#endif
        }

        private void OnCloseCallback()
        {
            _button.interactable = true;
            Time.timeScale = 1;
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }
    }
}