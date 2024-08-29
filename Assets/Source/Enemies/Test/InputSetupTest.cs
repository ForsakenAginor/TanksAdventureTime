using System.Collections.Generic;
using Player;
using Player.Input;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class InputSetupTest : MonoBehaviour // Используется только в EnemyTesting сцене
    {
        [Header("Moving")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Transform _transform;

        [Header("Aiming")]
        [SerializeField] private Transform _cannon;
        [SerializeField] private Camera _camera;
        [SerializeField] private PidRegulator _pidRegulator = new ();

        [Header("Shooting")]
        [SerializeField] private float _shootCooldown;
        [SerializeField] private SpawnableProjectile _projectile;
        [SerializeField] private HitEffect _hitEffect;
        [SerializeField] private float _attackAngle;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _cannonBarrel;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _reactionMask;

        private PlayerInput _playerInput;
        private MovingInputHandler _movingSystem;
        private AimInputHandler _aimSystem;
        private FireInputHandler _fireSystem;

        private void OnValidate()
        {
            if (_cannonBarrel == null)
                return;

            _cannonBarrel.localEulerAngles = new Vector3(
                -_attackAngle,
                (float)ValueConstants.Zero,
                (float)ValueConstants.Zero);
        }

        private void Awake()
        {
            PlayerWeapon weapon = new PlayerWeapon(
                new PlayerProjectileFactory(
                    _projectile,
                    _hitEffect,
                    new OverlapExplosive(_reactionMask),
                    _attackAngle * Mathf.Deg2Rad,
                    null),
                _shootPoint,
                _transform,
                _maxDistance);

            _playerInput = new PlayerInput();
            _movingSystem = new MovingInputHandler(_playerInput, _rigidbody, _speed, _rotationSpeed);
            _aimSystem = new AimInputHandler(_playerInput, _cannon, _pidRegulator, _camera, _transform);
            _fireSystem = new FireInputHandler(_playerInput, weapon, _shootCooldown);

            _fireSystem.StartWorking();
        }

        private void FixedUpdate()
        {
            _movingSystem.Moving();
            _aimSystem.Aim();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3 forward = _shootPoint.forward;
            Vector3 currentPosition = _shootPoint.position;
            Gizmos.DrawLine(currentPosition, forward + currentPosition);
            Vector3 targetPosition = forward * _maxDistance + currentPosition;
            targetPosition.y = _transform.position.y;
            List<Vector3> points = forward.CalculateTrajectory(
                currentPosition,
                targetPosition,
                targetPosition - currentPosition,
                _attackAngle * Mathf.Deg2Rad);
            Gizmos.color = Color.black;

            for (int i = 1; i < points.Count; i++)
                Gizmos.DrawLine(points[i - 1], points[i]);
        }
    }
}