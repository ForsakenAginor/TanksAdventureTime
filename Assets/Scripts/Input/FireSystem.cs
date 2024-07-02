using System;
using UnityEngine;
using UnityEngine.UI;

public class FireSystem : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private AmmoPool _pool;
    [SerializeField] private float _projectileSpeed;

    private PlayerInput _playerInput;

    private void OnDestroy()
    {
        _playerInput.FireInputReceived -= OnInputReceived;        
        _button.onClick.RemoveListener(OnInputReceived);
    }

    public void Init(PlayerInput playerInput)
    {
        _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));

        _playerInput.FireInputReceived += OnInputReceived;
        _button.onClick.AddListener(OnInputReceived);
    }

    private void OnInputReceived()
    {
        _pool.Pull().Launch(_shootingPoint.position, _shootingPoint.forward * _projectileSpeed);
    }
}
