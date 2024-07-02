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
        _input.Player.Fire.started += OnFireInputReceived;
        _input.Player.Ability.started += OnAbilityInputReceived;
    }

    ~PlayerInput()
    {
        _input.Player.Rotate.performed -= OnRotateInputReceived;
        _input.Player.Fire.started -= OnFireInputReceived;
        _input.Player.Ability.started -= OnAbilityInputReceived;
    }

    public event Action<Vector2> RotationInputReceived;

    public event Action<Vector2> RotationMouseInputReceived;

    public event Action FireInputReceived;

    public event Action AbilityInputReceived;

    public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();

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

    private void OnAbilityInputReceived(InputAction.CallbackContext context)
    {
        AbilityInputReceived?.Invoke();
    }
}