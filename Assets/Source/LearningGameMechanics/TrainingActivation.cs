using SavingProgress;
using UnityEngine;

namespace LearningGameMechanics
{
    public class TrainingActivation : MonoBehaviour
    {
        [SerializeField] private TrainingMobilePlatform _trainingMobilePlatform;
        [SerializeField] private SaveService _saveService;

        private void OnEnable() => _saveService.Loaded += OnActivate;

        private void OnDisable() => _saveService.Loaded -= OnActivate;

        private void OnActivate()
        {
            int minValue = 0;
            int maxValue = 1;
            bool isTraining = true ? _saveService.CompletedTrainingOnMobile == maxValue :
                                     _saveService.CompletedTrainingOnMobile == minValue;

            if (isTraining == true) return;

            if (Application.isMobilePlatform)
                _trainingMobilePlatform.gameObject.SetActive(true);
        }
    }
}