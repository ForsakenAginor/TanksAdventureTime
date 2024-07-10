using System.Collections.Generic;
using Projectiles;
using UnityEngine;

public class TestWeaponSetup : MonoBehaviour
{
    [Header("Factory")]
    [SerializeField] private SpawnableProjectile _projectile;
    [SerializeField] private HitEffect _hitEffect;
    [SerializeField] private float _attackAngle;

    [Header("Weapon")]
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _cannonBarrel;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _reactionMask;

    private IWeapon _weapon;

    private void OnValidate()
    {
        _cannonBarrel.localEulerAngles = new Vector3(
            -_attackAngle,
            (float)ValueConstants.Zero,
            (float)ValueConstants.Zero);
    }

    private void Awake()
    {
        _weapon = new PlayerWeapon(
            new PlayerProjectileFactory(
                _projectile,
                _hitEffect,
                new OverlapExplosive(_reactionMask),
                _attackAngle * Mathf.Deg2Rad,
                OnAudioCreated),
            _shootPoint,
            _transform,
            _maxDistance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 forward = _shootPoint.forward;
        Vector3 currentPosition = _shootPoint.position;
        Gizmos.DrawLine(currentPosition, forward + currentPosition);
        Vector3 targetPosition = forward * _maxDistance;
        targetPosition.y = _transform.position.y;
        List<Vector3> points = forward.CalculateTrajectory(
            currentPosition,
            targetPosition,
            targetPosition - currentPosition,
            _attackAngle * Mathf.Deg2Rad);
        Gizmos.color = Color.black;

        for (int i = 1; i < points.Count; i++)
            Gizmos.DrawLine(points[i - 1], points[i]);
    }

    private void OnAudioCreated(AudioSource source)
    {
    }
}