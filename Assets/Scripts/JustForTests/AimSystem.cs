using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    [SerializeField] private TextMeshProUGUI _text3;
    [SerializeField] private GameObject _cannon;
    [SerializeField] private PidRegulator _pidController = new();
    [SerializeField] private Camera _camera;

    private PlayerInput _playerInput;
    private RotationMouseInputHandler _rotationMouseInput;
    private IRotationInputDataHandler[] _rotationInputSources;
    private Vector3 _aimDirection;
    private Plane _plane;

    private void Awake()
    {
        _playerInput = new();
        _rotationMouseInput = new RotationMouseInputHandler(_camera, transform, _playerInput);
        _rotationInputSources = new IRotationInputDataHandler[] { _rotationMouseInput, _playerInput };
        _plane = new Plane(Vector3.up, transform.position);

        _playerInput.RotationInputReceived += OnInputReceived;
        _playerInput.RotationMouseInputReceived += OnMouseInputReceived;
    }

    private void OnMouseInputReceived(Vector2 vector)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        _plane.Raycast(ray, out float enter);
        Vector3 point = ray.origin + ray.direction * enter;
        Vector3 rotatedDirection = (point - transform.position);
        Vector2 result = new(rotatedDirection.x, rotatedDirection.z);
        result.Normalize();
        float angle = _cannon.transform.localRotation.eulerAngles.y * Mathf.Deg2Rad;
        float x = (result.x * Mathf.Cos(angle)) - (result.y * Mathf.Sin(angle));
        float y = (result.y * Mathf.Cos(angle)) + (result.x * Mathf.Sin(angle));

        _aimDirection = new Vector3(x, 0, y);
    }

    private void OnInputReceived(Vector2 vector)
    {
        _aimDirection = new Vector3(vector.x, 0, vector.y);
    }

    private void FixedUpdate()
    {
        /*
        Vector2 rotationInput = GetRotationInput();

        if (rotationInput != Vector2.zero )
        {
            Vector3 temp = rotationInput.normalized;
            _text.text = temp.ToString();
            _aimDirection = new Vector3 (temp.x, 0 ,temp.y);
        }
        */

        if (_aimDirection == Vector3.zero)
            return;

        float y = _cannon.transform.localRotation.eulerAngles.y;
        float currentAngle = y <= 180f ? y : y - 360f;
        _text1.text = currentAngle.ToString();
        Vector3 currentPosition = new Vector3(Mathf.Sin(y * Mathf.Deg2Rad),0, Mathf.Cos(y * Mathf.Deg2Rad));
        _text2.text = currentPosition.ToString();
        float requiredAngle = currentAngle + Vector3.SignedAngle(currentPosition, _aimDirection, Vector3.up);
        _text3.text = requiredAngle.ToString();

        _cannon.transform.rotation = Quaternion.AngleAxis(
                                    _pidController.Tick(currentAngle, requiredAngle, Time.deltaTime) * Time.deltaTime ,
                                    Vector3.up)
                                    * _cannon.transform.rotation;   
    }

    private Vector2 GetRotationInput()
    {
        return _rotationInputSources.Select(o => o.ReadRotation()).FirstOrDefault(o => o != Vector2.zero);
    }
}
