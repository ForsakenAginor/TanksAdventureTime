using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UnPauseGameButton : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private GameObject _buttonHolder;
        [SerializeField] private Canvas[] _canvasesThatWillBeEnabled;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _buttonHolder.SetActive(false);

            foreach (Canvas canvas in _canvasesThatWillBeEnabled)
                canvas.gameObject.SetActive(true);

            Time.timeScale = 1f;
        }
    }
}