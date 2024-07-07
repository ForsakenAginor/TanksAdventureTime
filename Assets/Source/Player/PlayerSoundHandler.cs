using Assets.Source.Player.Input;
using System;
using UnityEngine;

namespace Assets.Source.Player
{
    public class PlayerSoundHandler
    {
        private readonly FireInputHandler _fireSystem;
        private readonly MovingInputHandler _movingSystem;
        private readonly AudioSource _shootingAudioSource;
        private readonly AudioSource _movingAudioSource;
        private readonly int _volumeMultiplier = 2;

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

        private void OnShotFired()
        {
            _shootingAudioSource.Play();
        }

        private void OnMoveStarted()
        {
            float currentVolume = _movingAudioSource.volume;
            _movingAudioSource.volume = currentVolume * _volumeMultiplier;
        }

        private void OnMoveEnded()
        {
            float currentVolume = _movingAudioSource.volume;
            _movingAudioSource.volume = currentVolume / _volumeMultiplier;
        }
    }
}