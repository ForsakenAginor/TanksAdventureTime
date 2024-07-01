using System;
using UnityEngine;

public class PlayerInput : IRotationInputDataHandler
{
    private InputSystem _input;

    public PlayerInput()
    {
        _input = new();
        _input.Enable();
        _input.Player.Rotate.started += OnRotateInputStarted;
    }

    public event Action<Vector2> RotationInputReceived;

    public event Action<Vector2> RotationMouseInputReceived;

    public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();

    public Vector2 ReadRotation() => _input.Player.Rotate.ReadValue<Vector2>();

    private void OnRotateInputStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log(context.control.device);
        Vector2 input = context.ReadValue<Vector2>();

        if (context.control.device.ToString() != "Mouse:/Mouse")
            RotationInputReceived?.Invoke(input);
        else
            RotationMouseInputReceived?.Invoke(input);
    }
}