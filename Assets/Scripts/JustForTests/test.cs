using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    [SerializeField] private GameObject _cannon;
    [SerializeField] private PidRegulator _pidController = new();
    [SerializeField] private float _aimingSpeed;

    private PlayerInput _playerInput;
    private Vector3 _aimDirection;

    private void Awake()
    {
        _playerInput = new();
    }

    private void FixedUpdate()
    {
        if (_playerInput.ReadRotation() != Vector2.zero)
        {
            Vector3 temp = _playerInput.ReadRotation().normalized;
            _text.text = temp.ToString();
            _aimDirection = new Vector3 (temp.x, 0 ,temp.y);
            return;
        }

        Vector3 forward = Vector3.forward;

        float y = _cannon.transform.localRotation.eulerAngles.y;
        float currentAngle = y <= 180f ? y : y - 360f;
        _text1.text = currentAngle.ToString();
        float requiredAngle = Vector3.SignedAngle(Vector3.forward, _aimDirection, Vector3.up);
        _text2.text = requiredAngle.ToString();

        /*
        _text.text = _aimDirection.ToString();
        Vector3 forward = _cannon.transform.forward;

        float currentAngle = Vector3.SignedAngle(transform.forward, forward, Vector3.up);
        _text1.text = currentAngle.ToString();
        float requiredAngle = currentAngle + Vector3.SignedAngle(forward, _aimDirection, Vector3.up);
        _text2.text = requiredAngle.ToString();
       
        */
        _cannon.transform.rotation = Quaternion.AngleAxis(_pidController.Tick(currentAngle, requiredAngle, Time.deltaTime) * Time.deltaTime , Vector3.up)
                                    * _cannon.transform.rotation;   
    }
}
