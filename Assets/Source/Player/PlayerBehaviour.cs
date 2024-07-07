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

        public void Init(MovingInputHandler movingSystem, AimInputHandler aimSystem, PlayerSoundHandler playerSoundHandler)
        {
            _movingSystem = movingSystem != null ? movingSystem : throw new ArgumentNullException(nameof(movingSystem));
            _aimSystem = aimSystem != null ? aimSystem : throw new ArgumentNullException(nameof(aimSystem));
            _playerSoundHandler = playerSoundHandler != null ? playerSoundHandler : throw new ArgumentNullException(nameof(_playerSoundHandler));
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
        }

        public void Continue()
        {
            if (_playerSoundHandler == null)
                throw new Exception("PlayerInitializer class not initialized yet");

            _isWorking = true;
            _playerSoundHandler.Continue();
        }
    }
}