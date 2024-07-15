using System;
using System.Collections.Generic;
using Characters;
using Enemies;
using UnityEngine;

namespace PlayerHelpers
{
    [RequireComponent(typeof(CharacterAnimation))]
    public class PlayerHelperSetup : MonoBehaviour, ISwitchable<IDamageableTarget>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _viewPoint;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _thinkDelay;
        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _walls;
        [SerializeField] private AudioSource _fireSound;
        [Range(0f, 2f)]
        [SerializeField] private float _minPitch = 1f;
        [Range(0f, 2f)]
        [SerializeField] private float _maxPitch = 1.3f;
        [SerializeField] private ParticleSystem _shootingEffect;

        private CharacterAnimation _animation;

        private List<ISwitchable<IDamageableTarget>> _switchableObjects;
        private FiniteStateMachine<CharacterState> _machine;
        private CharacterRotator _rotator;
        private CharacterThinker _thinker;
        private TargetSwitcher _switcher;
        private PlayerHelperPresenter _presenter;
        private IWeapon _weapon;
        private IFieldOfView _fieldOfView;

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

        public void Init(List<IDamageableTarget> targets, Action<AudioSource> audioCreationCallback)
        {
            _animation = GetComponent<CharacterAnimation>();

            _machine = new FiniteStateMachine<CharacterState>();
            _rotator = new CharacterRotator(_rotationSpeed, _rotationPoint, null);
            _thinker = new CharacterThinker(_thinkDelay);
            _fieldOfView = CreateFieldOfView();

            _weapon = new MachineGun(
                _viewPoint,
                null,
                new AudioPitcher(_fireSound, _minPitch, _maxPitch),
                _shootingEffect);

            _switcher = new TargetSwitcher(
                targets,
                new List<ISwitchable<IDamageableTarget>>()
                {
                    (ISwitchable<IDamageableTarget>)_weapon,
                    (ISwitchable<IDamageableTarget>)_fieldOfView,
                    _rotator,
                    this,
                },
                _rotationPoint,
                new ImaginaryFieldOfView(CreateFieldOfView()));

            _presenter = new PlayerHelperPresenter(_machine, _thinker, _switcher);

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

            _presenter.Enable();
        }

        public void Switch(IDamageableTarget target)
        {
            _target = target;
        }

        private IFieldOfView CreateFieldOfView()
        {
            return new StandardFieldOfView(null, _viewPoint, _attackRadius, _walls);
        }
    }
}