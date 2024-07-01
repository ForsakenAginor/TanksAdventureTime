using System;
using UnityEngine;

public class PlayerInput : IRotationInputDataHandler
{
    private InputSystem _input;

    public PlayerInput()
    {
        _input = new();
        _input.Enable();
        UnityEngine.InputSystem.Utilities.ReadOnlyArray<UnityEngine.InputSystem.InputBinding> hz = _input.Player.Rotate.bindings;
    }

    public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();
    public Vector2 ReadRotation() => _input.Player.Rotate.ReadValue<Vector2>();    
}