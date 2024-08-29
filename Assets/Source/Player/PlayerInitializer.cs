using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Player.HealthSystem;
using Projectiles;
using Shops;
using UnityEngine;

namespace Player
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
        [SerializeField] private PidRegulator _pidRegulator = new ();

        [Header("Shooting")]
        [SerializeField] private AudioSource _shootingAudioSource;
        [SerializeField] private SpawnableProjectile _projectile;
        [SerializeField] private HitEffect _hitEffect;
        [SerializeField] private float _attackAngle;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _cannonBarrel;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _reactionMask;
        [SerializeField] private MuzzleFlashCreator _flashCreator;
        [SerializeField] private ParticleSystem _flashEffectPrefab;
        [SerializeField] private ShootingCooldownView _cooldownView;

        [Header("Player")] private PlayerBehaviour _player;

        [Header("HealthSystem")] [SerializeField]
        private HealthView[] _healthViews;

        private PlayerDamageTaker _playerDamageTaker;
        private Health _health;

        [Header("InputHandlers")] private MovingInputHandler _movingSystem;
        private AimInputHandler _aimSystem;
        private FireInputHandler _fireSystem;

        [Header("Other")] [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        private PlayerInput _playerInput;

        private void OnValidate()
        {
            _cannonBarrel.localEulerAngles = new Vector3(
                -_attackAngle,
                (float)ValueConstants.Zero,
                (float)ValueConstants.Zero);
        }

        private void OnDestroy()
        {
            _playerInput?.DisposeInputSystem();
        }

        public void Init(
            Dictionary<GoodNames, object> purchasedData,
            PlayerDamageTaker playerDamageTaker,
            PlayerBehaviour player,
            Action<AudioSource> onAudioCreated)
        {
            _playerDamageTaker = playerDamageTaker != null
                ? playerDamageTaker
                : throw new ArgumentNullException(nameof(playerDamageTaker));
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));

            if (purchasedData == null)
                throw new ArgumentNullException(nameof(purchasedData));

            PlayerWeapon weapon = new PlayerWeapon(
                new PlayerProjectileFactory(
                    _projectile,
                    _hitEffect,
                    new OverlapExplosive(_reactionMask),
                    _attackAngle * Mathf.Deg2Rad,
                    onAudioCreated),
                _shootPoint,
                _rigidbody.transform,
                _maxDistance);

            _playerInput = new ();
            _movingSystem = new (_playerInput, _rigidbody, _speed, _rotationSpeed);
            _aimSystem = new (_playerInput, _cannon, _pidRegulator, _camera, _rigidbody.transform);
            _fireSystem = new (_playerInput, weapon, (float)purchasedData[GoodNames.ReloadSpeed]);

            _cooldownView.Init(_fireSystem, (float)purchasedData[GoodNames.ReloadSpeed]);

            PlayerSoundHandler playerSoundHandler = new (
                _fireSystem,
                _movingSystem,
                _shootingAudioSource,
                _movingAudioSource);
            _flashCreator.Init(_flashEffectPrefab, _shootPoint, _fireSystem);

            var movingParticleEffect = Instantiate(_movingParticleEffectPrefab, _movingEffectSpawnPoint);
            OnMovingSmokeEffectHandler onMovingSmokeEffectHandler = new (movingParticleEffect, _movingSystem);

            _player.Init(_movingSystem, _aimSystem, playerSoundHandler, _fireSystem, _playerDamageTaker);

            _health = new Health((int)purchasedData[GoodNames.Health]);
            VirtualCameraShaker shaker = new (_virtualCamera, destroyCancellationToken);
            _playerDamageTaker.Init(_health, shaker);
            _healthViews.ToList().ForEach(o => o.Init(_health));
        }
    }
}