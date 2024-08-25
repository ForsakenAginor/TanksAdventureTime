using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Training : ActivitySwitch
{
    [SerializeField] private Transform _inputObject;
    [SerializeField] private Transform _backGroundPanel;

    protected InputSystem InputSystem { get; private set; }

    public bool IsPress { get; private set; } = false;

    public abstract event Action Canceled;

    private void Awake()
    {
        InputSystem = new();
        InputSystem.Enable();
    }

    public void EnableInputObject()
    {
        gameObject.SetActive(true);
        _inputObject.gameObject.SetActive(true);
        _backGroundPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DisableInputObject()
    {
        _inputObject.gameObject.SetActive(false);
    }

    public void InputActive(InputAction.CallbackContext callbackContext)
    {
        _backGroundPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
        IsPress = true;
        TurnOff();
    }
}