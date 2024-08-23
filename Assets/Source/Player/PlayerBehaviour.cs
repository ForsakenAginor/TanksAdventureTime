using Assets.Source.Player.HealthSystem;
using Assets.Source.Player.Input;
using System;
using UnityEngine;

namespace Assets.Source.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
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

        public void Init(MovingInputHandler movingSystem, AimInputHandler aimSystem,
            PlayerSoundHandler playerSoundHandler, FireInputHandler fireSystem, PlayerDamageTaker playerDamageTaker)
        {
            _movingSystem = movingSystem != null ? movingSystem : throw new ArgumentNullException(nameof(movingSystem));
            _aimSystem = aimSystem != null ? aimSystem : throw new ArgumentNullException(nameof(aimSystem));
            _playerSoundHandler = playerSoundHandler != null ? playerSoundHandler : throw new ArgumentNullException(nameof(_playerSoundHandler));
            _fireSystem = fireSystem != null ? fireSystem : throw new ArgumentNullException(nameof(fireSystem));
            _playerDamageTaker = playerDamageTaker != null ? playerDamageTaker : throw new ArgumentNullException(nameof(playerDamageTaker));
            _isWorking = true;
        }

        public void Stop()
        {
            if (_aimSystem == null || _playerSoundHandler == null)
                throw new Exception("PlayerInitializer class not initialized yet");

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
                throw new Exception("PlayerInitializer class not initialized yet");

            _isWorking = true;
            _playerSoundHandler.Continue();
            _fireSystem.StartWorking();
            _playerDamageTaker.Continue();
        }
    }
}