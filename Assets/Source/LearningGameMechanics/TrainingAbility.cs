using System;
using UnityEngine.InputSystem;

public class TrainingAbility : Training
{
    public override event Action Canceled;

    private void OnEnable()
    {
        InputSystem.Player.Ability.started += InputActive;
        InputSystem.Player.Ability.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Ability.started -= InputActive;
        InputSystem.Player.Ability.canceled -= OnCanceled;
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        Canceled?.Invoke();
    }
}