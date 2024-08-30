using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearningGameMechanics
{
    public abstract class Training : TrainingImage
    {
        private const int MinTimeScale = 0;
        private const int MaxTimeScale = 1;

        protected const int FirstPositionValue = 50;
        protected const int SecondPositionValue = 0;
        protected const float FirstDuration = 0.5f;
        protected const int SecondDuration = 1;

        [SerializeField] private Transform _inputObject;
        [SerializeField] private Transform _backGroundPanel;

        public event Action Canceled;

        public bool IsPress { get; private set; } = false;

        protected InputSystem InputSystem { get; private set; }

        private void Awake()
        {
            InputSystem = new();
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

        protected void OnInputActive(InputAction.CallbackContext callbackContext)
        {
            _backGroundPanel.gameObject.SetActive(false);
            Time.timeScale = MaxTimeScale;
            IsPress = true;
            TurnOff();
        }

        protected void OnCanceled(InputAction.CallbackContext context)
        {
            Canceled?.Invoke();
        }
    }
}