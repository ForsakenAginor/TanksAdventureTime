using Assets.Source.Player.Ammunition;
using Assets.Source.Player.Input;
using UnityEngine;

namespace Assets.Source.Player
{
    public class PlayerInitializer : MonoBehaviour
    {
        [Header("Moving")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;

        [Header("Aiming")]
        [SerializeField] private Transform _cannon;
        [SerializeField] private Camera _camera;
        [SerializeField] private PidRegulator _pidRegulator = new();

        [Header("Shooting")]
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private AmmoPool _pool;
        [SerializeField] private float _projectileSpeed;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _player;

        public void Init()
        {
            PlayerInput playerInput = new();
            MovingInputHandler movingSystem = new(playerInput, _rigidbody, _speed, _rotationSpeed);
            AimInputHandler aimSystem = new(playerInput, _cannon, _pidRegulator, _camera, _rigidbody.transform);
            FireInputHandler fireSystem = new(playerInput, _shootingPoint, _pool, _projectileSpeed);
            AbilityInputHandler abilitySystem = new(playerInput);

            _player.Init(movingSystem, aimSystem);
        }
    }
}