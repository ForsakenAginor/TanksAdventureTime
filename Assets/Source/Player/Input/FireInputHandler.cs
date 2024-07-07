using Assets.Source.Player.Ammunition;
using System;
using UnityEngine;

namespace Assets.Source.Player.Input
{
    public class FireInputHandler
    {
        private readonly Transform _shootingPoint;
        private readonly AmmoPool _pool;
        private readonly float _projectileSpeed;
        private readonly PlayerInput _playerInput;

        public FireInputHandler(PlayerInput playerInput, Transform shootingPoint, AmmoPool pool, float projectileSpeed)
        {
            _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));
            _shootingPoint = shootingPoint != null ? shootingPoint : throw new ArgumentNullException(nameof(shootingPoint));
            _pool = pool != null ? pool : throw new ArgumentNullException(nameof(pool));
            _projectileSpeed = projectileSpeed > 0 ? projectileSpeed : throw new ArgumentOutOfRangeException(nameof(projectileSpeed));

            _playerInput.FireInputReceived += OnInputReceived;
        }

        ~FireInputHandler()
        {
            _playerInput.FireInputReceived -= OnInputReceived;
        }

        public event Action ShotFired;

        private void OnInputReceived()
        {
            _pool.Pull().Launch(_shootingPoint.position, _shootingPoint.forward * _projectileSpeed);
            ShotFired?.Invoke();
        }
    }
}