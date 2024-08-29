using UnityEngine;
using UnityEngine.UI;

namespace LearningGameMechanics
{
    public abstract class TrainingImage : MonoBehaviour
    {
        [SerializeField] private Image _imageMobilePlatform;
        [SerializeField] private Transform _description;

        public Transform ImageTransform => _imageMobilePlatform.transform;

        private void Start() => OnTrainingStart();

        protected abstract void OnTrainingStart();

        public void TurnOn()
        {
            _description.gameObject.SetActive(true);
            _imageMobilePlatform.gameObject.SetActive(true);
        }

        public void TurnOff()
        {
            _description.gameObject.SetActive(false);
            _imageMobilePlatform.gameObject.SetActive(false);
        }
    }
}