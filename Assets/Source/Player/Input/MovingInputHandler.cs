using System;
using UnityEngine;

namespace Player
{
    public class MovingInputHandler
    {
        private readonly float _speed;
        private readonly float _rotationSpeed;
        private readonly Transform _transform;
        private readonly Rigidbody _rigidbody;
        private readonly PlayerInput _playerInput;

        private bool _isMoved;

        public MovingInputHandler(PlayerInput playerInput, Rigidbody rigidbody, float speed, float rotationSpeed)
        {
            _playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
            _rigidbody = rigidbody != null ? rigidbody : throw new ArgumentNullException(nameof(rigidbody));
            _transform = _rigidbody.transform;
            _speed = speed > 0 ? speed : throw new ArgumentOutOfRangeException(nameof(speed));
            _rotationSpeed = rotationSpeed > 0
                ? rotationSpeed
                : throw new ArgumentOutOfRangeException(nameof(rotationSpeed));
        }

        public event Action MoveStarted;

        public event Action MoveEnded;

        public void Moving()
        {
            if (_playerInput == null)
                return;

            Vector2 input = _playerInput.ReadMovement();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            if (input == Vector2.zero)
            {
                SetIsMoved(false);
                return;
            }

            Vector3 movingDirection = Vector3.zero;

            if (Mathf.Approximately(input.y, 0f) == false)
                movingDirection = (_transform.forward * input.y).normalized;

            _rigidbody.velocity = movingDirection * (_speed * Time.deltaTime);
            _transform.Rotate(Vector3.up, input.x * Time.deltaTime * _rotationSpeed);
            SetIsMoved(true);
        }

        public void CancelMove()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void SetIsMoved(bool isMoved)
        {
            if (isMoved != _isMoved && _isMoved == false)
            {
                _isMoved = isMoved;
                MoveStarted?.Invoke();
            }
            else if (isMoved != _isMoved && _isMoved == true)
            {
                _isMoved = isMoved;
                MoveEnded?.Invoke();
            }
        }
    }
}