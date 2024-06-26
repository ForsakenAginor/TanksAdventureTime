using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _pointSpawnBullet;
    [SerializeField] private Bullet _bulletPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Shoot();
    }

    private void Shoot()
    {
        Quaternion quaternion = transform.rotation;
        Instantiate(_bulletPrefab, _pointSpawnBullet.position, quaternion);
    }
}