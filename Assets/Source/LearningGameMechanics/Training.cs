using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LearningGameMechanics
{
    public abstract class Training : TrainingImage
    {
        private const int MinTimeScale = 0;
        private const int MaxTimeScale = 1;

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

        public void EnableInputObject()
        {
            gameObject.SetActive(true);
            _inputObject.gameObject.SetActive(true);
            _backGroundPanel.gameObject.SetActive(true);
            Time.timeScale = MinTimeScale;
        }

        public void DisableInputObject()
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