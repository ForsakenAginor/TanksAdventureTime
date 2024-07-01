using System;
using UnityEngine;

public class PlayerInput : IRotationInputDataHandler
{
    private InputSystem _input;

    public PlayerInput()
    {
        _input = new();
        _input.Enable();
        _input.Player.Rotate.performed += OnRotateInputPerformed;
    }

    public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();
    public Vector2 ReadRotation() => _input.Player.Rotate.ReadValue<Vector2>();    

    private void OnRotateInputPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log(context.control.device);
    }
}