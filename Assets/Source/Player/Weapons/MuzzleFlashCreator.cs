using System;
using UnityEngine;

namespace Player
{
    public class MuzzleFlashCreator : MonoBehaviour
    {
        private ParticleSystem _effectPrefab;
        private Transform _spawnPoint;
        private FireInputHandler _inputHandler;
        private ParticleSystem _effect;

        private void OnDestroy()
        {
            _inputHandler.ShotFired -= OnShotFired;
        }

        public void Init(ParticleSystem effect, Transform spawnPoint, FireInputHandler inputHandler)
        {
            _effectPrefab = effect != null ? effect : throw new ArgumentNullException(nameof(effect));
            _spawnPoint = spawnPoint != null ? spawnPoint : throw new ArgumentNullException(nameof(spawnPoint));
            _inputHandler = inputHandler != null ? inputHandler : throw new ArgumentNullException(nameof(inputHandler));

            _inputHandler.ShotFired += OnShotFired;
        }

        private void OnShotFired()
        {
            if (_effect == null)
                _effect = Instantiate(_effectPrefab, _spawnPoint);
            else
                _effect.Play();
        }
    }
}