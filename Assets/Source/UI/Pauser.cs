using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.UI
{
    [RequireComponent(typeof(Button))]
    public class Pauser : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Time.timeScale = 0f;
        }
    }
}