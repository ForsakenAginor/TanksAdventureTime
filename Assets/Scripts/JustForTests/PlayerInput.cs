using UnityEngine;


public class PlayerInput
{
    private InputSystem _input;

    public PlayerInput()
    {
        _input = new ();
        _input.Enable();
    }

    public Vector2 ReadMovement() => _input.Player.Move.ReadValue<Vector2>();
    public Vector2 ReadRotation() => _input.Player.Rotate.ReadValue<Vector2>();
}
