using System;
using UnityEngine;

namespace Player.Input
{
    public class AimInputHandler
    {
        private readonly PlayerInput _playerInput;
        private readonly Transform _cannon;
        private readonly PidRegulator _pidController;
        private readonly Camera _camera;
        private readonly Transform _transform;

        private Plane _plane;
        private Vector3 _aimDirection;

        public AimInputHandler(
            PlayerInput playerInput,
            Transform cannon,
            PidRegulator pidController,
            Camera camera,
            Transform transform)
        {
            _playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
            _cannon = cannon != null ? cannon : throw new ArgumentNullException(nameof(cannon));
            _pidController = pidController ?? throw new ArgumentNullException(nameof(pidController));
            _camera = camera != null ? camera : throw new ArgumentNullException(nameof(camera));
            _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
            _plane = new Plane(Vector3.up, _transform.position);

            _playerInput.RotationInputReceived += OnInputReceived;
            _playerInput.RotationMouseInputReceived += OnMouseInputReceived;
        }

        ~AimInputHandler()
        {
            _playerInput.RotationInputReceived -= OnInputReceived;
            _playerInput.RotationMouseInputReceived -= OnMouseInputReceived;
        }

        public void Aim()
        {
            if (_playerInput == null || _aimDirection == Vector3.zero)
                return;

            RotateCannon();
        }

        public void CancelAim()
        {
            _aimDirection = Vector3.zero;
        }

        private void RotateCannon()
        {
            float y = _cannon.localRotation.eulerAngles.y;
            float currentAngle = y <= 180f ? y : y - 360f;
            Vector3 currentPosition = new Vector3(Mathf.Sin(y * Mathf.Deg2Rad), 0, Mathf.Cos(y * Mathf.Deg2Rad));
            float requiredAngle = currentAngle + Vector3.SignedAngle(currentPosition, _aimDirection, Vector3.up);

            _cannon.rotation = Quaternion.AngleAxis(
                                   _pidController.Tick(currentAngle, requiredAngle, Time.deltaTime) * Time.deltaTime,
                                   Vector3.up)
                               * _cannon.rotation;
        }

        private void OnMouseInputReceived(Vector2 vector)
        {
            var ray = _camera.ScreenPointToRay(vector);

            _plane.Raycast(ray, out float enter);
            Vector3 point = ray.origin + ray.direction * enter;
            Vector3 rotatedDirection = point - _transform.position;
            Vector2 result = new (rotatedDirection.x, rotatedDirection.z);
            result.Normalize();
            float angle = _transform.transform.localRotation.eulerAngles.y * Mathf.Deg2Rad;
            float x = result.x * Mathf.Cos(angle) - result.y * Mathf.Sin(angle);
            float y = result.y * Mathf.Cos(angle) + result.x * Mathf.Sin(angle);

            _aimDirection = new Vector3(x, 0, y);
        }

        private void OnInputReceived(Vector2 vector)
        {
            _aimDirection = new Vector3(vector.x, 0, vector.y);
        }
    }
}