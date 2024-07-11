using Projectiles;
using UnityEngine;

namespace Assets.Source.Player.Weapons
{
    public class PlayerWeapon : IWeapon
    {
        private readonly IProjectileFactory Factory;
        private readonly Transform ShootPoint;
        private readonly Transform Transform;
        private readonly float MaxDistance;

        public PlayerWeapon(
            IProjectileFactory factory,
            Transform shootPoint,
            Transform transform,
            float maxDistance)
        {
            Factory = factory;
            ShootPoint = shootPoint;
            Transform = transform;
            MaxDistance = maxDistance;
        }

        public void Shoot()
        {
            Vector3 forward = ShootPoint.forward;
            Vector3 currentPosition = ShootPoint.position;
            Vector3 targetPosition = forward * MaxDistance + currentPosition;
            targetPosition.y = Transform.position.y;
            Factory.Create(currentPosition, targetPosition, targetPosition - currentPosition, forward);
        }
    }
}