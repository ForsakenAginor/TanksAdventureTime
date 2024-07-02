using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    [Header("Aiming")]
    [SerializeField] private Transform _cannon;
    [SerializeField] private Camera _camera;
    [SerializeField] private PidRegulator _pidRegulator = new ();

    [Header("Shooting")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private AmmoPool _pool;
    [SerializeField] private float _projectileSpeed;

    [Header("Player")]
    [SerializeField] private Player _player;

    public void Init()
    {
        PlayerInput playerInput = new ();
        MovingSystem movingSystem = new (playerInput, _rigidbody ,_transform, _speed, _rotationSpeed);
        AimSystem aimSystem = new (playerInput, _cannon, _pidRegulator, _camera, _transform);
        FireSystem fireSystem = new (playerInput, _shootingPoint, _pool, _projectileSpeed);
        AbilitySystem abilitySystem = new (playerInput);

        _player.Init(movingSystem, aimSystem);
    }
}