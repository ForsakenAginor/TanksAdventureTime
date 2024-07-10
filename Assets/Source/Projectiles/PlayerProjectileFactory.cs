using System;
using UnityEngine;

namespace Projectiles
{
    public class PlayerProjectileFactory : ProjectileFactory, IProjectileFactory
    {
        public PlayerProjectileFactory(
            SpawnableProjectile projectile,
            HitEffect hitTemplate,
            IExplosive explosive,
            float angleRadian,
            Action<AudioSource> audioCreationCallback)
            : base(projectile, hitTemplate, explosive, angleRadian, audioCreationCallback)
        {
        }

        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward)
        {
            Create(position).Move(direction, forward, CreateExplosion);
        }
    }
}