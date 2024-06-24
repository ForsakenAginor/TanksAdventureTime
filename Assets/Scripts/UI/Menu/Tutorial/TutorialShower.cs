using UnityEngine;

namespace Assets.Scripts.UI.Menu.Tutorial
{
    public class TutorialShower : MonoBehaviour
    {
        [SerializeField] private Transform _holderCanvas;
        [SerializeField] private GameObject _tutorialPrefab;

        private void Start()
        {
            TutorialData tutorialData = new ();

            if (tutorialData.GetTutorialCompletionStatus() == false)
                Instantiate(_tutorialPrefab, _holderCanvas);
        }
    }
}