using System;
using UnityEngine;

public class FireSystem
{
    private readonly Transform _shootingPoint;
    private readonly AmmoPool _pool;
    private readonly float _projectileSpeed;
    private readonly PlayerInput _playerInput;

    public FireSystem(PlayerInput playerInput, Transform shootingPoint, AmmoPool pool, float projectileSpeed)
    {
        _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));
        _shootingPoint = shootingPoint != null ? shootingPoint : throw new ArgumentNullException(nameof(shootingPoint));
        _pool = pool != null ? pool : throw new ArgumentNullException(nameof(pool));
        _projectileSpeed = projectileSpeed > 0 ? projectileSpeed : throw new ArgumentOutOfRangeException(nameof(projectileSpeed));

        _playerInput.FireInputReceived += OnInputReceived;
    }

    ~FireSystem()
    {
        _playerInput.FireInputReceived -= OnInputReceived;        
    }

    private void OnInputReceived()
    {
        _pool.Pull().Launch(_shootingPoint.position, _shootingPoint.forward * _projectileSpeed);
    }
}