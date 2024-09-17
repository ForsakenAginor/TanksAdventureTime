using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearningGameMechanics
{
    public abstract class Training : TrainingImage
    {
        public const int FirstPositionValue = 50;
        public const int SecondPositionValue = 0;
        public const float FirstDuration = 0.5f;
        public const int SecondDuration = 1;

        private const int MinTimeScale = 0;
        private const int MaxTimeScale = 1;

        [SerializeField] private Transform _inputObject;
        [SerializeField] private Transform _backGroundPanel;

        public event Action Canceled;

        protected InputSystem InputSystem { get; private set; }

        private void Awake()
        {
            InputSystem = new ();
            InputSystem.Enable();
        }

        public void EnableTraining()
        {
            gameObject.SetActive(true);
            _inputObject.gameObject.SetActive(true);
            _backGroundPanel.gameObject.SetActive(true);
            Time.timeScale = MinTimeScale;
        }

        public void DisableTraining()
        {
            _inputObject.gameObject.SetActive(false);
        }

        public void OnInputActive(InputAction.CallbackContext _)
        {
            _backGroundPanel.gameObject.SetActive(false);
            Time.timeScale = MaxTimeScale;
            TurnOff();
        }

        public void OnCanceled(InputAction.CallbackContext _)
        {
            Canceled?.Invoke();
        }
    }
}