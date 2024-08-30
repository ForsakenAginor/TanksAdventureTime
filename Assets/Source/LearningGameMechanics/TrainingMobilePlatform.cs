using System.Collections.Generic;
using SavingProgress;
using UnityEngine;
using UnityEngine.UI;

namespace LearningGameMechanics
{
    public class TrainingMobilePlatform : MonoBehaviour
    {
        private const int Deductible = 1;

        [SerializeField] private Button _playButton;
        [SerializeField] private SaveService _saveService;
        [SerializeField] private List<Training> _trainings = new ();

        private int _currentTrainingIndex = 0;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnFillTrainings);

            foreach (Training item in _trainings)
                item.Canceled += OnNextTraining;
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnFillTrainings);

            foreach (Training item in _trainings)
                item.Canceled -= OnNextTraining;
        }

        private void OnFillTrainings()
        {
            foreach (var training in _trainings)
                training.DisableTraining();

            _trainings[_currentTrainingIndex].TurnOn();
            _trainings[_currentTrainingIndex].EnableTraining();
        }

        private void OnNextTraining()
        {
            _trainings[_currentTrainingIndex].enabled = false;

            if (_currentTrainingIndex == _trainings.Count - Deductible)
            {
                _saveService.SaveCompletedTrainingMobile(true);
                gameObject.SetActive(false);
                return;
            }

            _trainings[++_currentTrainingIndex].TurnOn();
            _trainings[_currentTrainingIndex].EnableTraining();
        }
    }
}