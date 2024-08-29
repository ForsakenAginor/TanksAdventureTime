using System;
using Player.Input;
using UnityEngine;

namespace Player
{
    public class PlayerSoundHandler
    {
        private readonly FireInputHandler _fireSystem;
        private readonly MovingInputHandler _movingSystem;
        private readonly AudioSource _shootingAudioSource;
        private readonly AudioSource _movingAudioSource;
        private readonly int _volumeMultiplier = 2;
        private bool _isBoosted;

        public PlayerSoundHandler(FireInputHandler fireSystem, MovingInputHandler movingSystem, AudioSource shootingAudioSource, AudioSource movingAudioSource)
        {
            _fireSystem = fireSystem != null ? fireSystem : throw new ArgumentNullException(nameof(fireSystem));
            _movingSystem = movingSystem != null ? movingSystem : throw new ArgumentNullException(nameof(movingSystem));
            _shootingAudioSource = shootingAudioSource != null ? shootingAudioSource : throw new ArgumentNullException(nameof(shootingAudioSource));
            _movingAudioSource = movingAudioSource != null ? movingAudioSource : throw new ArgumentNullException(nameof(movingAudioSource));

            _fireSystem.ShotFired += OnShotFired;
            _movingSystem.MoveStarted += OnMoveStarted;
            _movingSystem.MoveEnded += OnMoveEnded;
        }

        ~PlayerSoundHandler()
        {
            _fireSystem.ShotFired -= OnShotFired;
            _movingSystem.MoveStarted -= OnMoveStarted;
            _movingSystem.MoveEnded -= OnMoveEnded;
        }

        public void Stop()
        {
            _fireSystem.ShotFired -= OnShotFired;
            _movingSystem.MoveStarted -= OnMoveStarted;
            _movingSystem.MoveEnded -= OnMoveEnded;

            _movingAudioSource.Pause();

            if (_isBoosted)
                ReduceEngineVolume();
        }

        public void Continue()
        {
            _fireSystem.ShotFired += OnShotFired;
            _movingSystem.MoveStarted += OnMoveStarted;
            _movingSystem.MoveEnded += OnMoveEnded;

            _movingAudioSource.UnPause();
        }

        private void OnShotFired()
        {
            _shootingAudioSource.Play();
        }

        private void OnMoveStarted()
        {
            BoostEngineVolume();
        }

        private void OnMoveEnded()
        {
            ReduceEngineVolume();
        }

        private void BoostEngineVolume()
        {
            float currentVolume = _movingAudioSource.volume;
            _movingAudioSource.volume = currentVolume * _volumeMultiplier;
            _isBoosted = true;
        }

        private void ReduceEngineVolume()
        {
            float currentVolume = _movingAudioSource.volume;
            _movingAudioSource.volume = currentVolume / _volumeMultiplier;
            _isBoosted = false;
        }
    }
}