﻿using System;
using System.Collections.Generic;
using Characters;
using Projectiles;
using UnityEngine;

namespace PlayerHelpers
{
    [RequireComponent(typeof(CharacterAnimation))]
    public class PlayerHelperSetup : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _viewPoint;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private Transform _drawPoint;
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
        [SerializeField] private List<SerializedPair<PlayerHelperTypes, SerializedPair<GameObject, AudioClip>>> _views;

        private GameObject _gameObject;
        private CharacterAnimation _animation;

        private List<ISwitchable<IDamageableTarget>> _switchableObjects;
        private FiniteStateMachine<CharacterState> _machine;
        private CharacterRotator _rotator;
        private CharacterThinker _thinker;
        private CircleDrawer _drawer;
        private TargetSwitcher _switcher;
        private PlayerHelperPresenter _presenter;
        private IWeapon _weapon;
        private IFieldOfView _fieldOfView;
        private SwitchableExplosive _explosive;

        private void OnDestroy()
        {
            _presenter?.Disable();
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

        public void Init()
        {
            gameObject.SetActive(false);
        }

        private void InitParts()
        {
            _machine = new FiniteStateMachine<CharacterState>();
            _rotator = new CharacterRotator(_rotationSpeed, _rotationPoint, null);
            _thinker = new CharacterThinker(_thinkDelay);
            _drawer = new CircleDrawer(_attackRadius, _drawPoint, _line);
        }

        private void InitSwitcher(
            List<IDamageableTarget> targets,
            PlayerHelperTypes type,
            Action<AudioSource> audioCreationCallback)
        {
            _fieldOfView = CreateFieldOfView(type);

            _switchableObjects = new List<ISwitchable<IDamageableTarget>>()
            {
                (ISwitchable<IDamageableTarget>)_fieldOfView,
                _drawer,
                _rotator,
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
                new ImaginaryFieldOfView(CreateFieldOfView(type)));
        }

        private void InitPresenter(Action<(Action onEnable, Action onDisable)> presenterInitCallback)
        {
            (Action onEnable, Action onDisable) activationHandler = (() =>
            {
                _thinker.Start();
                _drawer.StartDraw();
                _explosive?.StartTracking();
                _gameObject.SetActive(true);
            }, () =>
            {
                _thinker.Stop();
                _drawer.StopDraw();
                _explosive?.StopTracking();
                _gameObject.SetActive(false);
            });

            _presenter = new PlayerHelperPresenter(_machine, _thinker, _switcher, activationHandler);
            presenterInitCallback?.Invoke(activationHandler);
        }

        private void InitMachine()
        {
            _machine.AddStates(
                new Dictionary<Type, FiniteStateMachineState<CharacterState>>()
                {
                    {
                        typeof(CharacterIdleState),
                        new CharacterIdleState(_machine, _fieldOfView, _animation)
                    },
                    {
                        typeof(CharacterAttackState),
                        new SingleAttackState(
                            _machine,
                            _fieldOfView,
                            _rotator,
                            _weapon,
                            new AudioPitcher(_fireSound, _minPitch, _maxPitch),
                            _animation)
                    },
                });

            _animation.Init(_animator, () => _machine.SetState(typeof(CharacterIdleState)));
        }

        private IFieldOfView CreateFieldOfView(PlayerHelperTypes type)
        {
            return type switch
            {
                PlayerHelperTypes.MachineGun => new StandardFieldOfView(null, _viewPoint, _attackRadius, _walls),
                PlayerHelperTypes.Grenade => new MortarFieldOfView(
                    null,
                    _viewPoint,
                    _attackRadius,
                    _walls,
                    _attackAngle,
                    _projectile.ColliderRadius,
                    (float)ValueConstants.One),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private IWeapon CreateWeapon(
            PlayerHelperTypes type,
            Action<AudioSource> audioCreationCallback,
            Action<IExplosive> explosiveCreationCallback)
        {
            return type switch
            {
                PlayerHelperTypes.MachineGun => CreateMachineGun(),
                PlayerHelperTypes.Grenade => CreateGrenade(audioCreationCallback, explosiveCreationCallback),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private IWeapon CreateMachineGun()
        {
            ActivateView(PlayerHelperTypes.MachineGun);
            return new MachineGun(
                _viewPoint,
                null,
                _shootingEffect);
        }

        private IWeapon CreateGrenade(
            Action<AudioSource> audioCreationCallback,
            Action<IExplosive> explosiveCreationCallback)
        {
            _explosive = new SwitchableExplosive(_projectile.ColliderRadius);
            explosiveCreationCallback?.Invoke(_explosive);
            ActivateView(PlayerHelperTypes.Grenade);

            _viewPoint.localEulerAngles = new Vector3(
                -_attackAngle,
                (float)ValueConstants.Zero,
                (float)ValueConstants.Zero);

            return new Mortar(
                _viewPoint,
                null,
                new PlayerHelperProjectileFactory(
                    _projectile,
                    _hitTemplate,
                    _explosive,
                    _explosive,
                    _attackAngle * Mathf.Deg2Rad,
                    audioCreationCallback));
        }

        private void ActivateView(PlayerHelperTypes type)
        {
            SerializedPair<GameObject, AudioClip> pair =
                _views.Find(item => item.Key == type).Value;
            pair.Key.SetActive(true);
            _fireSound.clip = pair.Value;
        }
    }
}