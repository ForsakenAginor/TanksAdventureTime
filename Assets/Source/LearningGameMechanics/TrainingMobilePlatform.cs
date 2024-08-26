using System.Collections.Generic;
using Assets.Source.SavingData;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.LearningGameMechanics
{
    public class TrainingMobilePlatform : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private SaveService _saveService;
        [SerializeField] private List<Training> _trainings = new();

        private int _currentTrainingIndex = 0;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(FillTraining);

            for (int i = 0; i < _trainings.Count; i++)
            {
                _trainings[i].Canceled += NextTraining;
            }
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(FillTraining);

            for (int i = 0; i < _trainings.Count; i++)
            {
                _trainings[i].Canceled -= NextTraining;
            }
        }

        private void FillTraining()
        {
            foreach (var training in _trainings)
            {
                training.DisableInputObject();
            }

            _trainings[_currentTrainingIndex].TurnOn();
            _trainings[_currentTrainingIndex].EnableInputObject();
        }

        private void NextTraining()
        {
            int element = 1;
            _trainings[_currentTrainingIndex].enabled = false;

            if (_currentTrainingIndex == _trainings.Count - element)
            {
                _saveService.SetCompletedTrainingMobileData(true);
                gameObject.SetActive(false);
                return;
            }

            _trainings[++_currentTrainingIndex].TurnOn();
            _trainings[_currentTrainingIndex].EnableInputObject();
        }
    }
}