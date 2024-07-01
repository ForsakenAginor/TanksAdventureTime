using System;
using UnityEngine;

public class RotationMouseInputHandler : IRotationInputDataHandler
{
    private readonly Camera _camera;
    private readonly Plane _plane;
    private readonly Transform _cannon;
    private PlayerInput _playerInput;

    public RotationMouseInputHandler(Camera camera, Transform cannon, PlayerInput playerInput)
    {
        _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
        _cannon = cannon != null ? cannon : throw new ArgumentNullException(nameof(cannon));
        _plane = new Plane(Vector3.up, _cannon.position);
        _playerInput = playerInput;
    }

    public Vector2 ReadRotation()
    {
        Vector2 rotationInput = _playerInput.ReadRotation();

        if (rotationInput == Vector2.zero)        
            return Vector2.zero;        

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        _plane.Raycast(ray, out float enter);
        Vector3 point = ray.origin + ray.direction * enter;
        Vector3 rotatedDirection = (point - _cannon.position);
        Vector2 result = new(rotatedDirection.x, rotatedDirection.z);
        result.Normalize();
        float angle = _cannon.transform.localRotation.eulerAngles.y * Mathf.Deg2Rad;
        float x = (result.x * Mathf.Cos(angle)) - (result.y * Mathf.Sin(angle));
        float y = (result.y * Mathf.Cos(angle)) + (result.x * Mathf.Sin(angle));

        return new Vector2(x, y);
    }
}
