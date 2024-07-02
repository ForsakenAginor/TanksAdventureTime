using Assets.Source.Player.Input;
using System;
using UnityEngine;

namespace Assets.Source.Player
{
    public class Player : MonoBehaviour
    {
        private MovingInputHandler _movingSystem;
        private AimInputHandler _aimSystem;

        private void FixedUpdate()
        {
            if (_movingSystem == null || _aimSystem == null)
                return;

            _movingSystem.Moving();
            _aimSystem.Aim();
        }

        public void Init(MovingInputHandler movingSystem, AimInputHandler aimSystem)
        {
            _movingSystem = movingSystem != null ? movingSystem : throw new ArgumentNullException(nameof(movingSystem));
            _aimSystem = aimSystem != null ? aimSystem : throw new ArgumentNullException(nameof(aimSystem));
        }
    }
}