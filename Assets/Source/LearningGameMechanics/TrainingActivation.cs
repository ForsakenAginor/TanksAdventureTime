using SavingProgress;
using UnityEngine;

namespace LearningGameMechanics
{
    public class TrainingActivation : MonoBehaviour
    {
        private const int MaxValue = 1;

        [SerializeField] private TrainingMobilePlatform _trainingMobilePlatform;
        [SerializeField] private SaveService _saveService;

        private void OnEnable() => _saveService.Loaded += OnActivate;

        private void OnDisable() => _saveService.Loaded -= OnActivate;

        private void OnActivate()
        {
            bool isTraining = _saveService.CompletedTrainingOnMobile == MaxValue;

            if (isTraining == true)
                return;

            if (Application.isMobilePlatform)
                _trainingMobilePlatform.gameObject.SetActive(true);
        }
    }
}