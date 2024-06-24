using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class ExitButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            Application.Quit();
        }
    }
}