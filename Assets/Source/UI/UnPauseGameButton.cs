using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UI
{
    public class UnPauseGameButton : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private GameObject _buttonHolder;
        [SerializeField] private GameObject[] _gameObjectsThatWillBeEnabled;

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

            foreach (GameObject gameObject in _gameObjectsThatWillBeEnabled)
                gameObject.SetActive(true);

            Time.timeScale = 1f;
        }
    }
}