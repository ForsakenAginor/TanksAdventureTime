using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TraningMoveTank : Training
{
    private InputSystem _inputSystem;

    private void OnEnable()
    {
        _inputSystem.Player.Move.started += Move1;
        _inputSystem.Player.Move.performed += Move1;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Move.started -= Move1;
        _inputSystem.Player.Move.performed -= Move1;
    }

    private void Move1(InputAction.CallbackContext obj)
    {
        TurnOff();
        Debug.Log("Started");
    }

    private void Move2(InputAction.CallbackContext obj)
    {
        TurnOff();
        Debug.Log("Performed");
    }
}
