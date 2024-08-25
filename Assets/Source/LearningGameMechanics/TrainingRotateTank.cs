using System;
using UnityEngine.InputSystem;

public class TrainingRotateTank : Training
{
    public override event Action Canceled;

    private void OnEnable()
    {
        InputSystem.Player.Rotate.started += InputActive;
        InputSystem.Player.Rotate.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Rotate.started -= InputActive;
        InputSystem.Player.Rotate.canceled -= OnCanceled;
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        Canceled?.Invoke();
    }
}