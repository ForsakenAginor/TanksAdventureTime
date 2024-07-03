using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimation))]
    public class EnemySetup : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _viewPoint;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _thinkDelay;
        [SerializeField] private EnemyTypes _enemyType;
        [SerializeField] private bool _isDebug;

        [Header("Field Of View")]
        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _walls;
        [Range(0f, 90f)]
        [SerializeField] private float _attackAngle = 45f;
        [SerializeField] private float _mortarTrajectoryHeightOffset;

        [Header("Weapon")]
        [SerializeField] private AudioSource _sound;
        [Range(0f, 2f)]
        [SerializeField] private float _minPitch = 1f;
        [Range(0f, 2f)]
        [SerializeField] private float _maxPitch = 1.3f;
        [SerializeField] private ParticleSystem _shootingEffect;
        [SerializeField] private HitEffect _hitTemplate;
        [SerializeField] private MortarProjectile _projectile;
        [SerializeField] private AimParticle _aimTemplate;
        [SerializeField] private ProjectileTypes _projectileType;
        [SerializeField] private float _distanceBetween = 0f;
        [SerializeField] private int _clusterCount = 0;

        [Header("Debug")]
        [SerializeField] private TargetTest _debugTarget;

        private EnemyAnimation _animation;

        private FiniteStateMachine<EnemyState> _machine;
        private IWeapon _weapon;
        private IFieldOfView _fieldOfView;
        private IPlayerTarget _target;
        private EnemyRotator _rotator;
        private EnemyThinker _thinker;
        private CancellationToken _token;

        private EnemyPresenter _presenter;

        private void OnValidate()
        {
            if (_enemyType != EnemyTypes.Mortar)
                return;

            _viewPoint.localEulerAngles = new Vector3(
                -_attackAngle,
                (float)ValueConstants.Zero,
                (float)ValueConstants.Zero);
        }

        private void OnDestroy()
        {
            _presenter?.Disable();
        }

        public void Init(IPlayerTarget target)
        {
            _target = target;
            _animation = GetComponent<EnemyAnimation>();
            _machine = new FiniteStateMachine<EnemyState>();
            _rotator = new EnemyRotator(_rotationSpeed, _transform, _target);
            _thinker = new EnemyThinker(destroyCancellationToken, _thinkDelay);
            _presenter = new EnemyPresenter(_machine, _thinker);

            _weapon = GetWeapon(target);
            _fieldOfView = GetFieldOfView();

            _machine.AddStates(
                new Dictionary<Type, FiniteStateMachineState<EnemyState>>()
                {
                    {
                        typeof(EnemyIdleState),
                        new EnemyIdleState(_machine, _animation, _fieldOfView)
                    },
                    {
                        typeof(EnemyAttackState),
                        new EnemyAttackState(_machine, _animation, _fieldOfView, _rotator, _weapon)
                    },
                });

            _animation.Init(_animator, () => _machine.SetState(typeof(EnemyIdleState)));

            _presenter.Enable();
        }

        private IWeapon GetWeapon(IDamageableTarget target)
        {
            AudioPitcher sound = new AudioPitcher(_sound, _minPitch, _maxPitch);

            switch (_enemyType)
            {
                case EnemyTypes.Standard:
                case EnemyTypes.Bunker:
                    return new Gun(_hitTemplate, _viewPoint, _target, _shootingEffect, sound);

                case EnemyTypes.Mortar:
                    return new Mortar(
                        _viewPoint,
                        _target,
                        sound,
                        new ProjectileFactory(
                            _projectile,
                            _hitTemplate,
                            _aimTemplate,
                            target,
                            _projectileType,
                            _attackAngle * Mathf.Deg2Rad,
                            _distanceBetween,
                            _clusterCount));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IFieldOfView GetFieldOfView()
        {
            return _enemyType switch
            {
                EnemyTypes.Standard => new StandardFieldOfView(_target, _viewPoint, _attackRadius, _walls),
                EnemyTypes.Mortar => new MortarFieldOfView(
                    _target,
                    _viewPoint,
                    _attackRadius,
                    _walls,
                    _attackAngle,
                    _projectile.ColliderRadius,
                    _mortarTrajectoryHeightOffset),
                EnemyTypes.Bunker => new BunkerFieldOfView(
                    _target,
                    _viewPoint,
                    _attackRadius,
                    _walls,
                    _attackAngle / (int)ValueConstants.Two),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnDrawGizmos()
        {
            if (_isDebug == false)
                return;

            if (_viewPoint == null || _transform == null)
                return;

            if (_debugTarget == null)
                return;

            Vector3 currentPosition = _viewPoint.position;
            Vector3 targetPosition = _debugTarget.Position;
            Vector3 forward = _viewPoint.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentPosition, _attackRadius);

            switch (_enemyType)
            {
                case EnemyTypes.Standard:
                    break;

                case EnemyTypes.Mortar:
                    Vector3 direction = targetPosition - currentPosition;
                    float angleRadian = _attackAngle * Mathf.Rad2Deg;
                    Vector3 newForward = forward.RotateAlongY(direction);
                    List<Vector3> points = newForward.CalculateTrajectory(currentPosition, targetPosition, direction, angleRadian);

                    if (points.Count <= 0)
                        return;

                    Vector3 middleHeightPosition = points[points.Count / (int)ValueConstants.Two];
                    middleHeightPosition.y += _mortarTrajectoryHeightOffset;

                    Gizmos.color = Color.blue;

                    for (int i = 1; i < points.Count; i++)
                        Gizmos.DrawLine(points[i - 1], points[i]);

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(currentPosition, middleHeightPosition);
                    Gizmos.DrawLine(middleHeightPosition, targetPosition);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(currentPosition, forward + currentPosition);
                    Gizmos.DrawLine(currentPosition, newForward + currentPosition);

                    if (_projectileType != ProjectileTypes.Triple)
                        return;

                    Gizmos.color = Color.black;
                    points.Clear();
                    Vector3 cross = Vector3.Cross(direction, Vector3.up).normalized * _distanceBetween;
                    Vector3 leftDirection = cross + targetPosition - currentPosition;
                    Vector3 rightDirection = -cross + targetPosition - currentPosition;
                    points = forward.RotateAlongY(leftDirection)
                        .CalculateTrajectory(currentPosition, cross + targetPosition, leftDirection, angleRadian);

                    for (int i = 1; i < points.Count; i++)
                        Gizmos.DrawLine(points[i - 1], points[i]);

                    points = forward.RotateAlongY(rightDirection)
                        .CalculateTrajectory(currentPosition, -cross + targetPosition, rightDirection, angleRadian);

                    for (int i = 1; i < points.Count; i++)
                        Gizmos.DrawLine(points[i - 1], points[i]);

                    break;

                case EnemyTypes.Bunker:
                    float angle = _attackAngle / (int)ValueConstants.Two;
                    forward *= _attackRadius;
                    Vector3 leftPoint = currentPosition + Quaternion.Euler(new Vector3(0f, angle, 0f)) * forward;
                    Vector3 rightPoint = currentPosition + Quaternion.Euler(new Vector3(0f, -angle, 0f)) * forward;

                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(currentPosition, forward + currentPosition);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(currentPosition, leftPoint);
                    Gizmos.DrawLine(currentPosition, rightPoint);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(currentPosition, targetPosition);
        }
    }
}