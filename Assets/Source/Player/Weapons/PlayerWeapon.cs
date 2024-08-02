using Projectiles;
using UnityEngine;

namespace Assets.Source.Player.Weapons
{
    public class PlayerWeapon : IWeapon
    {
        private readonly IProjectileFactory _factory;
        private readonly Transform _shootPoint;
        private readonly Transform _transform;
        private readonly float _maxDistance;

        public PlayerWeapon(
            IProjectileFactory factory,
            Transform shootPoint,
            Transform transform,
            float maxDistance)
        {
            _factory = factory;
            _shootPoint = shootPoint;
            _transform = transform;
            _maxDistance = maxDistance;
        }

        public void Shoot()
        {
            Vector3 forward = _shootPoint.forward;
            Vector3 currentPosition = _shootPoint.position;
            Vector3 targetPosition = forward * _maxDistance + currentPosition;
            targetPosition.y = _transform.position.y;
            _factory.Create(currentPosition, targetPosition, targetPosition - currentPosition, forward);
        }
    }
}