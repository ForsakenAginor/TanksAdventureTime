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

    private void Awake()
    {
        _playerInput = new();
        _rotationMouseInput = new RotationMouseInputHandler(_camera, transform, _playerInput);
        _rotationInputSources = new IRotationInputDataHandler[] { _rotationMouseInput, _playerInput };
    }

    private void FixedUpdate()
    {
        Vector2 rotationInput = GetRotationInput();

        if (rotationInput != Vector2.zero )
        {
            Vector3 temp = rotationInput.normalized;
            _text.text = temp.ToString();
            _aimDirection = new Vector3 (temp.x, 0 ,temp.y);
        }

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
