using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sound
{
    public class UISlideSoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSourceOpener;
        [SerializeField] private Button _closeButton;
        [SerializeField] private AudioSource _audioSourceCloser;

        private void OnEnable()
        {
            _audioSourceOpener.Play();
            _closeButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _audioSourceCloser.Play();
        }
    }
}