using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput
    {
        private readonly InputSystem _input;

        public PlayerInput()
        {
            _input = new InputSystem();
            _input.Enable();
            _input.Player.Rotate.performed += OnRotateInputReceived;
            _input.Player.Fire.started += OnFireInputReceived;
        }

        ~PlayerInput()
        {
            _input.Player.Rotate.performed -= OnRotateInputReceived;
            _input.Player.Fire.started -= OnFireInputReceived;
        }

        public event Action<Vector2> RotationInputReceived;

        public event Action<Vector2> RotationMouseInputReceived;

        public event Action FireInputReceived;

        public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();

        public void DisposeInputSystem()
        {
            _input.Dispose();
        }

        private void OnRotateInputReceived(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();

            if (context.control.device is Mouse == false)
                RotationInputReceived?.Invoke(input);
            else
                RotationMouseInputReceived?.Invoke(input);
        }

        private void OnFireInputReceived(InputAction.CallbackContext context)
        {
            FireInputReceived?.Invoke();
        }
    }
}