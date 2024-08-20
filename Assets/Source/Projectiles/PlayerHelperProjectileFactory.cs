using System;
using Characters;
using UnityEngine;

namespace Projectiles
{
    public class PlayerHelperProjectileFactory : ProjectileFactory, IProjectileFactory
    {
        private readonly ISwitchable<(ITarget projectile, Action explosionCallback)> _switchable;

        public PlayerHelperProjectileFactory(
            SpawnableProjectile projectile,
            HitEffect hitTemplate,
            IExplosive explosive,
            ISwitchable<(ITarget projectile, Action explosionCallback)> switchable,
            float angleRadian,
            Action<AudioSource> audioCreationCallback)
            : base(projectile, hitTemplate, explosive, angleRadian, audioCreationCallback)
        {
            _switchable = switchable;
        }

        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward)
        {
            SpawnableProjectile projectile = Create(position);
            _switchable.Switch((projectile, projectile.Explode));
            projectile.Move(direction, forward, CreateExplosion);
        }
    }
}