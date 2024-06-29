using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new();
    }

    private void FixedUpdate()
    {
        //_text.text = _playerInput.ReadRotation().ToString();
        Vector2 vector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _text.text = vector.ToString();
    }
}
