using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _audioSource.Play();
        }
    }
}