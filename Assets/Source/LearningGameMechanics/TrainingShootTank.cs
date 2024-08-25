using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrainingShootTank : Training
{
    public override event Action Canceled;

    private void OnEnable()
    {
        InputSystem.Player.Fire.started += InputActive;
        InputSystem.Player.Fire.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Fire.started -= InputActive;
        InputSystem.Player.Fire.canceled -= OnCanceled;
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("SHoot");
        Canceled?.Invoke();
    }
}