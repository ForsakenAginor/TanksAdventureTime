using System;
using UnityEngine;

namespace Player
{
    public class OnMovingSmokeEffectHandler
    {
        private readonly ParticleSystem _effect;
        private readonly MovingInputHandler _movingInput;

        public OnMovingSmokeEffectHandler(ParticleSystem effect, MovingInputHandler movingInput)
        {
            _effect = effect != null ? effect : throw new ArgumentNullException(nameof(effect));
            _movingInput = movingInput ?? throw new ArgumentNullException(nameof(movingInput));

            _movingInput.MoveStarted += OnMoveStarted;
            _movingInput.MoveEnded += OnMoveEnded;
        }

        ~OnMovingSmokeEffectHandler()
        {
            _movingInput.MoveStarted -= OnMoveStarted;
            _movingInput.MoveEnded -= OnMoveEnded;
        }

        private void OnMoveStarted()
        {
            _effect.Play();
        }

        private void OnMoveEnded()
        {
            _effect.Stop();
        }
    }
}