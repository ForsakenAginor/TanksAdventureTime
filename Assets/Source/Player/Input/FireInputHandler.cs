using Assets.Source.Player.Weapons;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Source.Player.Input
{
    public class FireInputHandler
    {
        private readonly PlayerWeapon _playerWeapon;
        private readonly PlayerInput _playerInput;
        private readonly int _shootDelay;
        private bool _isReadyToFire = true;

        public FireInputHandler(PlayerInput playerInput, PlayerWeapon playerWeapon, float shootDelay)
        {
            _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));
            _playerWeapon = playerWeapon != null ? playerWeapon : throw new ArgumentNullException(nameof(playerWeapon));
            _shootDelay = shootDelay >= 0 ? (int)(shootDelay * 1000) : throw new ArgumentOutOfRangeException(nameof(shootDelay));

            _playerInput.FireInputReceived += OnInputReceived;
        }

        ~FireInputHandler()
        {
            _playerInput.FireInputReceived -= OnInputReceived;
        }

        public event Action ShotFired;

        private void OnInputReceived()
        {
            Shoot();
        }

        private async void Shoot()
        {
            if (_isReadyToFire == false)
                return;

            _isReadyToFire = false;
            _playerWeapon.Shoot();
            ShotFired?.Invoke();
            await UniTask.Delay(_shootDelay);
            _isReadyToFire = true;
        }
    }
}