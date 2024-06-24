using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu.Tutorial
{
    [RequireComponent(typeof(Button))]
    public class TutorialDataSaver : MonoBehaviour
    {
        private readonly TutorialData _tutorialData = new ();

        [SerializeField] private GameObject _holderPanel;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(SaveData);
        }

        private void OnDisable()
        {
            _button.onClick.AddListener(SaveData);
        }

        private void SaveData()
        {
            _tutorialData.SaveTutorialData();
            _holderPanel.SetActive(false);
        }
    }
}