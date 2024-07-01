using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput
{
    private InputSystem _input;

    public PlayerInput()
    {
        _input = new();
        _input.Enable();
        _input.Player.Rotate.performed += OnRotateInputReceived;
    }

    ~PlayerInput()
    {
        _input.Player.Rotate.performed -= OnRotateInputReceived;
    }

    public event Action<Vector2> RotationInputReceived;

    public event Action<Vector2> RotationMouseInputReceived;

    public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();

    private void OnRotateInputReceived(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (context.control.device is Mouse == false)
            RotationInputReceived?.Invoke(input);
        else
            RotationMouseInputReceived?.Invoke(input);
    }
}