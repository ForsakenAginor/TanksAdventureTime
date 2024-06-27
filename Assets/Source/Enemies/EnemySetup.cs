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
        [SerializeField] private EnemyTypes _type;
        [SerializeField] private bool _isDebug;

        [Header("Field Of View")]
        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _walls;
        [Range(0f, 90f)]
        [SerializeField] private float _attackAngle = 45f;
        [SerializeField] private float _mortarTrajectoryHeightOffset;

        [Header("Weapon")]
        [SerializeField] private AudioSource _sound;
        [SerializeField] private ParticleSystem _shootingEffect;
        [SerializeField] private HitEffect _hitEffect;
        [SerializeField] private MortarProjectile _projectile;

        [Header("Debug")]
        [SerializeField] private TargetTest _debugTarget;

        private EnemyAnimation _animation;

        private FiniteStateMachine<EnemyState> _machine;
        private IWeapon _weapon;
        private IFieldOfView _fieldOfView;
        private IPlayerDetector _detectorTarget;
        private EnemyRotator _rotator;
        private EnemyThinker _thinker;
        private CancellationToken _token;

        private EnemyPresenter _presenter;
        private IEnemyFactory<IWeapon> _weaponFactory;
        private IEnemyFactory<IFieldOfView> _fieldOfViewFactory;

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

            switch (_type)
            {
                case EnemyTypes.Standard:
                    break;

                case EnemyTypes.Mortar:
                    Vector3 gravity = Physics.gravity;
                    Vector3 direction = targetPosition - currentPosition;
                    float angleRadian = _attackAngle * Mathf.Rad2Deg;
                    Vector3 velocity = forward.CalculateVelocity(direction, angleRadian, gravity.y);
                    List<Vector3> points = new ();

                    for (float i = 0; i < Vector3.Distance(currentPosition, targetPosition); i += 0.2f)
                    {
                        Vector3 position = currentPosition + velocity * i + gravity * i * i / (float)ValueConstants.Two;

                        if (position.y < targetPosition.y)
                            break;

                        points.Add(position);
                    }

                    if (points.Count <= 0)
                        return;

                    Vector3 middleHeightPosition = points[points.Count / (int)ValueConstants.Two];
                    middleHeightPosition.y += _mortarTrajectoryHeightOffset;
                    Vector3 newForward = forward.RotateAlongY(direction);

                    Gizmos.color = Color.blue;

                    for (int i = 1; i < points.Count; i++)
                        Gizmos.DrawLine(points[i - 1], points[i]);

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(currentPosition, middleHeightPosition);
                    Gizmos.DrawLine(middleHeightPosition, targetPosition);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(currentPosition, forward + currentPosition);
                    Gizmos.DrawLine(currentPosition, newForward + currentPosition);
                    break;

                case EnemyTypes.Bunker:
                    float angle = _attackAngle / (int)ValueConstants.Two;
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

        private void OnValidate()
        {
            if (_type != EnemyTypes.Mortar)
                return;

            _viewPoint.localEulerAngles = new Vector3(-_attackAngle, (float)ValueConstants.Zero, (float)ValueConstants.Zero);
        }

        private void OnDestroy()
        {
            _presenter?.Disable();
        }

        public void Init(IPlayerDetector detector)
        {
            _detectorTarget = detector;
            _animation = GetComponent<EnemyAnimation>();
            _machine = new FiniteStateMachine<EnemyState>();
            _rotator = new EnemyRotator(_rotationSpeed, _transform, _detectorTarget);
            _thinker = new EnemyThinker(destroyCancellationToken, _thinkDelay);
            _presenter = new EnemyPresenter(_machine, _thinker);

            PrepareFactories();
            _weapon = _weaponFactory.Create(_type);
            _fieldOfView = _fieldOfViewFactory.Create(_type);

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

        private void PrepareFactories()
        {
            _weaponFactory = new WeaponFactory(
                _viewPoint,
                _detectorTarget,
                _hitEffect,
                _shootingEffect,
                _sound,
                _projectile,
                _attackAngle);
            _fieldOfViewFactory = new FieldOfViewFactory(
                _detectorTarget,
                _viewPoint,
                _attackRadius,
                _walls,
                _attackAngle,
                _mortarTrajectoryHeightOffset,
                _projectile ? _projectile.Radius : (float)ValueConstants.Zero);
        }
    }
}