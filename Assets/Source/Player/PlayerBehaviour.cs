using System;
using Player.HealthSystem;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private const string ExceptionMessage = "PlayerInitializer class not initialized yet";

        private MovingInputHandler _movingSystem;
        private AimInputHandler _aimSystem;
        private PlayerSoundHandler _playerSoundHandler;
        private FireInputHandler _fireSystem;
        private PlayerDamageTaker _playerDamageTaker;
        private bool _isWorking;

        private void FixedUpdate()
        {
            if (_movingSystem == null || _aimSystem == null)
                return;

            if (_isWorking == false)
                return;

            _movingSystem.Moving();
            _aimSystem.Aim();
        }

        public void Init(
            MovingInputHandler movingSystem,
            AimInputHandler aimSystem,
            PlayerSoundHandler playerSoundHandler,
            FireInputHandler fireSystem,
            PlayerDamageTaker playerDamageTaker)
        {
            _movingSystem = movingSystem ?? throw new ArgumentNullException(nameof(movingSystem));
            _aimSystem = aimSystem ?? throw new ArgumentNullException(nameof(aimSystem));
            _playerSoundHandler = playerSoundHandler ?? throw new ArgumentNullException(nameof(playerSoundHandler));
            _fireSystem = fireSystem ?? throw new ArgumentNullException(nameof(fireSystem));
            _playerDamageTaker = playerDamageTaker != null
                ? playerDamageTaker
                : throw new ArgumentNullException(nameof(playerDamageTaker));
            _isWorking = true;
        }

        public void Stop()
        {
            if (_aimSystem == null || _playerSoundHandler == null)
                throw new Exception(ExceptionMessage);

            _isWorking = false;
            _movingSystem.CancelMove();
            _playerSoundHandler.Stop();
            _aimSystem.CancelAim();
            _fireSystem.StopWorking();
            _playerDamageTaker.StopWorking();
        }

        public void Continue()
        {
            if (_playerSoundHandler == null)
                throw new Exception(ExceptionMessage);

            _isWorking = true;
            _playerSoundHandler.Continue();
            _fireSystem.StartWorking();
            _playerDamageTaker.Continue();
        }
    }
}