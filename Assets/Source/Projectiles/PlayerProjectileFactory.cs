using UnityEngine;

namespace Projectiles
{
    public class PlayerProjectileFactory : ProjectileFactory, IProjectileFactory
    {
        public PlayerProjectileFactory(
            SpawnableProjectile projectile,
            HitEffect hitTemplate,
            IExplosive explosive,
            float angleRadian)
            : base(projectile, hitTemplate, explosive, angleRadian)
        {
        }

        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward)
        {
            Create(position).Move(direction, forward, CreateExplosion);
        }
    }
}