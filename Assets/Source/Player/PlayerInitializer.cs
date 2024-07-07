using Assets.Source.Player.Ammunition;
using Assets.Source.Player.HealthSystem;
using Assets.Source.Player.Input;
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

        [Header("Aiming")]
        [SerializeField] private Transform _cannon;
        [SerializeField] private Camera _camera;
        [SerializeField] private PidRegulator _pidRegulator = new();

        [Header("Shooting")]
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private AmmoPool _pool;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] private AudioSource _shootingAudioSource;

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

        private void OnValidate()
        {
            if (_maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(_maxHealth));
        }

        public void Init(PlayerDamageTaker playerDamageTaker, PlayerBehaviour player)
        {
            _playerDamageTaker = playerDamageTaker != null ? playerDamageTaker : throw new ArgumentNullException(nameof(player));
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));

            PlayerInput playerInput = new();
            _movingSystem = new(playerInput, _rigidbody, _speed, _rotationSpeed);
            _aimSystem = new(playerInput, _cannon, _pidRegulator, _camera, _rigidbody.transform);
            _fireSystem = new(playerInput, _shootingPoint, _pool, _projectileSpeed);
            _abilitySystem = new(playerInput);

            PlayerSoundHandler playerSoundHandler = new (_fireSystem, _movingSystem, _shootingAudioSource, _movingAudioSource);

            _player.Init(_movingSystem, _aimSystem, playerSoundHandler);

            _health = new Health(_maxHealth);
            _playerDamageTaker.Init(_health);
            _healthViews.ToList().ForEach(o => o.Init(_health));
        }
    }
}