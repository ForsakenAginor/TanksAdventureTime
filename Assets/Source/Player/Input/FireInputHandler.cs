using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class FireInputHandler
    {
        private readonly PlayerWeapon _playerWeapon;
        private readonly PlayerInput _playerInput;
        private readonly int _shootDelay;
        private bool _isReadyToFire = true;
        private bool _isWorking = true;

        public FireInputHandler(PlayerInput playerInput, PlayerWeapon playerWeapon, float shootDelay)
        {
            _playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
            _playerWeapon = playerWeapon ?? throw new ArgumentNullException(nameof(playerWeapon));
            _shootDelay = shootDelay >= 0
                ? (int)TimeSpan.FromSeconds(shootDelay).TotalMilliseconds
                : throw new ArgumentOutOfRangeException(nameof(shootDelay));

            _playerInput.FireInputReceived += OnInputReceived;
        }

        ~FireInputHandler()
        {
            _playerInput.FireInputReceived -= OnInputReceived;
        }

        public event Action ShotFired;

        public void StartWorking() => _isWorking = true;

        public void StopWorking() => _isWorking = false;

        private void OnInputReceived()
        {
            if (_isWorking == false)
                return;

            bool isWebGLOnMobile = Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer;

            if (isWebGLOnMobile == false && EventSystem.current.IsPointerOverGameObject())
                return;

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