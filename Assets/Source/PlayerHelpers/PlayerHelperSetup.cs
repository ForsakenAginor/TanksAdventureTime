using System;
using System.Collections.Generic;
using Characters;
using Projectiles;
using UnityEngine;

namespace PlayerHelpers
{
    [RequireComponent(typeof(CharacterAnimation))]
    public class PlayerHelperSetup : MonoBehaviour, ISwitchable<IDamageableTarget>
    {
        [Header("Main")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _viewPoint;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _thinkDelay;
        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _walls;
        [SerializeField] private LineRenderer _line;

        [Header("Weapon")]
        [SerializeField] private AudioSource _fireSound;
        [Range(0f, 2f)]
        [SerializeField] private float _minPitch = 1f;
        [Range(0f, 2f)]
        [SerializeField] private float _maxPitch = 1.3f;
        [SerializeField] private ParticleSystem _shootingEffect;
        [SerializeField] private SpawnableProjectile _projectile;
        [SerializeField] private HitEffect _hitTemplate;
        [SerializeField] private float _attackAngle = 0f;
        [SerializeField] private List<SerializedPair<PlayerHelperTypes, GameObject>> _renderObject;

        private GameObject _gameObject;
        private CharacterAnimation _animation;

        private List<ISwitchable<IDamageableTarget>> _switchableObjects;
        private Dictionary<PlayerHelperTypes, Func<Action<AudioSource>, Action<IExplosive>, IWeapon>> _weapons;
        private FiniteStateMachine<CharacterState> _machine;
        private CharacterRotator _rotator;
        private CharacterThinker _thinker;
        private CircleDrawer _drawer;
        private TargetSwitcher _switcher;
        private PlayerHelperPresenter _presenter;
        private IWeapon _weapon;
        private IFieldOfView _fieldOfView;
        private IExplosive _explosive;

        private IDamageableTarget _target;

        private void OnDestroy()
        {
            _presenter.Disable();
        }

        private void OnDrawGizmos()
        {
            if (_rotationPoint == null)
                return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_rotationPoint.position, _attackRadius);

            if (_target == null)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_viewPoint.position, _target.Position);
        }

        public void Init(
            List<IDamageableTarget> targets,
            PlayerHelperTypes type,
            Action<AudioSource> audioCreationCallback,
            Action<(Action onEnable, Action onDisable)> presenterInitCallback)
        {
            _gameObject = gameObject;
            _animation = GetComponent<CharacterAnimation>();
            audioCreationCallback?.Invoke(_fireSound);

            InitParts();
            InitSwitcher(targets, type, audioCreationCallback);
            InitPresenter(presenterInitCallback);
            InitMachine();

            _presenter.Enable();
        }

        public void Switch(IDamageableTarget target)
        {
            _target = target;
        }

        private void InitParts()
        {
            _machine = new FiniteStateMachine<CharacterState>();
            _rotator = new CharacterRotator(_rotationSpeed, _rotationPoint, null);
            _thinker = new CharacterThinker(_thinkDelay);
            _drawer = new CircleDrawer(_attackRadius, _viewPoint, _line, _rotationPoint.position.y);
            _fieldOfView = CreateFieldOfView();
        }

        private void InitSwitcher(
            List<IDamageableTarget> targets,
            PlayerHelperTypes type,
            Action<AudioSource> audioCreationCallback)
        {
            _switchableObjects = new List<ISwitchable<IDamageableTarget>>()
            {
                (ISwitchable<IDamageableTarget>)_fieldOfView,
                _rotator,
                this,
            };
            _switchableObjects.Add(
                (ISwitchable<IDamageableTarget>)(_weapon = CreateWeapon(
                    type,
                    audioCreationCallback,
                    explosive => _switchableObjects.Add((ISwitchable<IDamageableTarget>)explosive))));

            _switcher = new TargetSwitcher(
                targets,
                _switchableObjects,
                _rotationPoint,
                new ImaginaryFieldOfView(CreateFieldOfView()));
        }

        private void InitPresenter(Action<(Action onEnable, Action onDisable)> presenterInitCallback)
        {
            (Action onEnable, Action onDisable) activationHandler = (() =>
            {
                _switcher.StartSearching();
                _thinker.Start();
                _drawer.StartDraw();
                _gameObject.SetActive(true);
            }, () =>
            {
                _switcher.StopSearching();
                _thinker.Stop();
                _drawer.StopDraw();
                _gameObject.SetActive(false);
            });

            _presenter = new PlayerHelperPresenter(_machine, _thinker, activationHandler);
            presenterInitCallback?.Invoke(activationHandler);
        }

        private void InitMachine()
        {
            _machine.AddStates(
                new Dictionary<Type, FiniteStateMachineState<CharacterState>>()
                {
                    {
                        typeof(CharacterIdleState),
                        new CharacterIdleState(_machine, _animation, _fieldOfView)
                    },
                    {
                        typeof(CharacterAttackState),
                        new CharacterAttackState(_machine, _animation, _fieldOfView, _rotator, _weapon)
                    },
                });

            _animation.Init(_animator, () => _machine.SetState(typeof(CharacterIdleState)));
        }

        private IFieldOfView CreateFieldOfView()
        {
            return new StandardFieldOfView(null, _viewPoint, _attackRadius, _walls);
        }

        private IWeapon CreateWeapon(
            PlayerHelperTypes type,
            Action<AudioSource> audioCreationCallback,
            Action<IExplosive> explosiveCreationCallback)
        {
            _weapons = new Dictionary<PlayerHelperTypes, Func<Action<AudioSource>, Action<IExplosive>, IWeapon>>()
            {
                {
                    PlayerHelperTypes.MachineGun, (_, _) =>
                    {
                        _renderObject
                            .Find(item => item.Key == PlayerHelperTypes.MachineGun)
                            .Value
                            .SetActive(true);

                        return new MachineGun(
                            _viewPoint,
                            null,
                            new AudioPitcher(_fireSound, _minPitch, _maxPitch),
                            _shootingEffect);
                    }
                },
                {
                    PlayerHelperTypes.Grenade,
                    (audioCallback, explosiveCallback) =>
                    {
                        IExplosive explosive = new Explosive(null);
                        explosiveCallback?.Invoke(explosive);
                        _renderObject
                            .Find(item => item.Key == PlayerHelperTypes.Grenade)
                            .Value
                            .SetActive(true);

                        _viewPoint.localEulerAngles = new Vector3(
                            -_attackAngle,
                            (float)ValueConstants.Zero,
                            (float)ValueConstants.Zero);

                        return new Mortar(
                            _viewPoint,
                            null,
                            new AudioPitcher(_fireSound, _minPitch, _maxPitch),
                            new PlayerProjectileFactory(
                                _projectile,
                                _hitTemplate,
                                explosive,
                                _attackAngle * Mathf.Deg2Rad,
                                audioCallback));
                    }
                }
            };

            return _weapons[type]?.Invoke(audioCreationCallback, explosiveCreationCallback);
        }
    }
}