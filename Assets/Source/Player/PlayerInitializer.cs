using Assets.Source.Player.HealthSystem;
using Assets.Source.Player.Input;
using Assets.Source.Player.MovingEffect;
using Assets.Source.Player.Weapons;
using Assets.Source.Sound.AudioMixer;
using Projectiles;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Player
{
    public class PlayerInitializer : MonoBehaviour
    {
        [Header("Moving")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private AudioSource _movingAudioSource;
        [SerializeField] private Transform _movingEffectSpawnPoint;
        [SerializeField] private ParticleSystem _movingParticleEffectPrefab;

        [Header("Aiming")]
        [SerializeField] private Transform _cannon;
        [SerializeField] private Camera _camera;
        [SerializeField] private PidRegulator _pidRegulator = new();

        [Header("Shooting")]
        [SerializeField] private float _shootCooldown;
        [SerializeField] private AudioSource _shootingAudioSource;
        [SerializeField] private SpawnableProjectile _projectile;
        [SerializeField] private HitEffect _hitEffect;
        [SerializeField] private float _attackAngle;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _cannonBarrel;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _reactionMask;
        [SerializeField] private MuzzleFlashCreator _flashCreator;
        [SerializeField] private ParticleSystem _flashhEffectPrefab;

        [Header("Player")]
        private PlayerBehaviour _player;

        [Header("HealthSystem")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private HealthView[] _healthViews;
        private PlayerDamageTaker _playerDamageTaker;
        private Health _health;

        [Header("InputHandlers")]
        private MovingInputHandler _movingSystem;
        private AimInputHandler _aimSystem;
        private FireInputHandler _fireSystem;
        private AbilityInputHandler _abilitySystem;

        [Header("Other")]
        private PlayerInput _playerInput;
        private SoundInitializer _soundInitializer;

        private void OnValidate()
        {
            if (_maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(_maxHealth));

            if(_shootCooldown < 0)
                throw new ArgumentOutOfRangeException(nameof(_shootCooldown));

            _cannonBarrel.localEulerAngles = new Vector3(
                                            -_attackAngle,
                                            (float)ValueConstants.Zero,
                                            (float)ValueConstants.Zero);
        }

        private void OnDestroy()
        {
            _playerInput?.DisposeInputSystem();
        }

        public void Init(PlayerDamageTaker playerDamageTaker, PlayerBehaviour player, SoundInitializer soundInitializer)
        {
            _playerDamageTaker = playerDamageTaker != null ? playerDamageTaker : throw new ArgumentNullException(nameof(player));
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));
            _soundInitializer = soundInitializer != null ? soundInitializer : throw new ArgumentNullException(nameof(soundInitializer));

            PlayerWeapon weapon = new PlayerWeapon(
                                    new PlayerProjectileFactory(
                                        _projectile,
                                        _hitEffect,
                                        new OverlapExplosive(_reactionMask),
                                        _attackAngle * Mathf.Deg2Rad,
                                        OnAudioCreated),
                                    _shootPoint,
                                    _rigidbody.transform,
                                    _maxDistance);

            _playerInput = new();
            _movingSystem = new(_playerInput, _rigidbody, _speed, _rotationSpeed);
            _aimSystem = new(_playerInput, _cannon, _pidRegulator, _camera, _rigidbody.transform);
            _fireSystem = new(_playerInput, weapon, _shootCooldown);
            _abilitySystem = new(_playerInput);

            PlayerSoundHandler playerSoundHandler = new(_fireSystem, _movingSystem, _shootingAudioSource, _movingAudioSource);
            _flashCreator.Init(_flashhEffectPrefab, _shootPoint, _fireSystem);

            var movingParticleEffect = Instantiate(_movingParticleEffectPrefab, _movingEffectSpawnPoint);
            OnMovingSmokeEffectHandler onMovingSmokeEffectHandler = new(movingParticleEffect, _movingSystem);

            _player.Init(_movingSystem, _aimSystem, playerSoundHandler);

            _health = new Health(_maxHealth);
            _playerDamageTaker.Init(_health);
            _healthViews.ToList().ForEach(o => o.Init(_health));
        }

        private void OnAudioCreated(AudioSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _soundInitializer.AddEffectSource(source);
        }
    }
}