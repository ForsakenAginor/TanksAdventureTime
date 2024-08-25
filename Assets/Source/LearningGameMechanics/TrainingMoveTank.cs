using System;
using UnityEngine.InputSystem;

public class TrainingMoveTank : Training
{
    public override event Action Canceled;

    private void OnEnable()
    {
        InputSystem.Player.Move.started += InputActive;
        InputSystem.Player.Move.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Move.started -= InputActive;
        InputSystem.Player.Move.canceled -= OnCanceled;
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        Canceled?.Invoke();
    }
}