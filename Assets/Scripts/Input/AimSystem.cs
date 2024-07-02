using System;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    [SerializeField] private GameObject _cannon;
    [SerializeField] private PidRegulator _pidController = new();
    [SerializeField] private Camera _camera;

    private PlayerInput _playerInput;
    private Vector3 _aimDirection;
    private Plane _plane;

    private void Awake()
    {
        _plane = new Plane(Vector3.up, transform.position);
    }

    private void FixedUpdate()
    {
        if (_playerInput == null || _aimDirection == Vector3.zero)
            return;

        RotateCannon();
    }

    private void OnDestroy()
    {
        _playerInput.RotationInputReceived -= OnInputReceived;
        _playerInput.RotationMouseInputReceived -= OnMouseInputReceived;
    }

    public void Init(PlayerInput playerInput)
    {
        _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));

        _playerInput.RotationInputReceived += OnInputReceived;
        _playerInput.RotationMouseInputReceived += OnMouseInputReceived;
    }

    private void RotateCannon()
    {
        float y = _cannon.transform.localRotation.eulerAngles.y;
        float currentAngle = y <= 180f ? y : y - 360f;
        Vector3 currentPosition = new Vector3(Mathf.Sin(y * Mathf.Deg2Rad), 0, Mathf.Cos(y * Mathf.Deg2Rad));
        float requiredAngle = currentAngle + Vector3.SignedAngle(currentPosition, _aimDirection, Vector3.up);

        _cannon.transform.rotation = Quaternion.AngleAxis(
                                    _pidController.Tick(currentAngle, requiredAngle, Time.deltaTime) * Time.deltaTime,
                                    Vector3.up)
                                    * _cannon.transform.rotation;
    }

    private void OnMouseInputReceived(Vector2 vector)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        _plane.Raycast(ray, out float enter);
        Vector3 point = ray.origin + ray.direction * enter;
        Vector3 rotatedDirection = (point - transform.position);
        Vector2 result = new(rotatedDirection.x, rotatedDirection.z);
        result.Normalize();
        float angle = transform.transform.localRotation.eulerAngles.y * Mathf.Deg2Rad;
        float x = (result.x * Mathf.Cos(angle)) - (result.y * Mathf.Sin(angle));
        float y = (result.y * Mathf.Cos(angle)) + (result.x * Mathf.Sin(angle));

        _aimDirection = new Vector3(x, 0, y);
    }

    private void OnInputReceived(Vector2 vector)
    {
        _aimDirection = new Vector3(vector.x, 0, vector.y);
    }
}
