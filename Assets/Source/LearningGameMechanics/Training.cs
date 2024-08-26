using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Source.LearningGameMechanics
{
    public abstract class Training : TrainingImage
    {
        [SerializeField] private Transform _inputObject;
        [SerializeField] private Transform _backGroundPanel;

        private float _minTimeScale = 0;
        private float _maxTimeScale = 1;

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
            Time.timeScale = _minTimeScale;
        }

        public void DisableInputObject()
        {
            _inputObject.gameObject.SetActive(false);
        }

        protected void OnInputActive(InputAction.CallbackContext callbackContext)
        {
            _backGroundPanel.gameObject.SetActive(false);
            Time.timeScale = _maxTimeScale;
            IsPress = true;
            TurnOff();
        }

        protected void OnCanceled(InputAction.CallbackContext context)
        {
            Canceled?.Invoke();
        }
    }
}