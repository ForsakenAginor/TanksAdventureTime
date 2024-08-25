using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Training : ImageDisplayControl
{
    [SerializeField] private Transform _inputObject;
    [SerializeField] private Transform _backGroundPanel;

    public abstract event Action Canceled;

    private float _minTimeScale = 0;
    private float _maxTimeScale = 1;
    protected InputSystem InputSystem { get; private set; }

    public bool IsPress { get; private set; } = false;


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
        Time.timeScale = _minTimeScale;
    }

    public void DisableInputObject()
    {
        _inputObject.gameObject.SetActive(false);
    }

    public void InputActive(InputAction.CallbackContext callbackContext)
    {
        _backGroundPanel.gameObject.SetActive(false);
        Time.timeScale = _maxTimeScale;
        IsPress = true;
        TurnOff();
    }
}