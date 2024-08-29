using System.Collections.Generic;
using SavingProgress;
using UnityEngine;
using UnityEngine.UI;

namespace LearningGameMechanics
{
    public class TrainingMobilePlatform : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private SaveService _saveService;
        [SerializeField] private List<Training> _trainings = new();

        private int _currentTrainingIndex = 0;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnFillTraining);

            for (int i = 0; i < _trainings.Count; i++)
            {
                _trainings[i].Canceled += OnNextTraining;
            }
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnFillTraining);

            for (int i = 0; i < _trainings.Count; i++)
            {
                _trainings[i].Canceled -= OnNextTraining;
            }
        }

        private void OnFillTraining()
        {
            foreach (var training in _trainings)
            {
                training.DisableInputObject();
            }

            _trainings[_currentTrainingIndex].TurnOn();
            _trainings[_currentTrainingIndex].EnableInputObject();
        }

        private void OnNextTraining()
        {
            int element = 1;
            _trainings[_currentTrainingIndex].enabled = false;

            if (_currentTrainingIndex == _trainings.Count - element)
            {
                _saveService.SaveCompletedTrainingMobile(true);
                gameObject.SetActive(false);
                return;
            }

            _trainings[++_currentTrainingIndex].TurnOn();
            _trainings[_currentTrainingIndex].EnableInputObject();
        }
    }
}