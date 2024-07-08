using System;
using Projectiles;
using UnityEngine;

public class TestWeaponSetup : MonoBehaviour
{
    [Header("Factory")] [SerializeField]
    private SpawnableProjectile _projectile;

    [SerializeField] private HitEffect _hitEffect;
    [SerializeField] private float _attackAngle;

    [Header("Weapon")] [SerializeField]
    private Transform _shootPoint;

    [SerializeField] private Transform _transform;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _maxDistance;

    private PlayerWeapon _weapon;

    private void Awake()
    {
        _weapon = new PlayerWeapon(
            new PlayerProjectileFactory(
                _projectile,
                _hitEffect,
                new OverlapExplosive(),
                _attackAngle * Mathf.Deg2Rad),
            _shootPoint,
            _transform,
            _line,
            _maxDistance,
            _attackAngle * Mathf.Deg2Rad);
    }

    private void OnDestroy()
    {
        _weapon.StopDrawTrajectory();
    }

    private void Update()
    {
        _weapon.DrawTrajectory();
    }
}