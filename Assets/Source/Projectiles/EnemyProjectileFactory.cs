using System;
using UnityEngine;

namespace Projectiles
{
    public class EnemyProjectileFactory : ProjectileFactory, IProjectileFactory
    {
        private readonly ObjectPool<AimParticle> _aimPool;
        private readonly ProjectileTypes _type;
        private readonly float _distanceBetween;
        private readonly int _clusterCount;

        public EnemyProjectileFactory(
            SpawnableProjectile projectile,
            HitEffect hitTemplate,
            AimParticle aimTemplate,
            IExplosive explosive,
            float angleRadian,
            Action<AudioSource> audioCreationCallback,
            ProjectileTypes type,
            float distanceBetweenExplosions,
            int clusterExplosionsCount)
            : base(projectile, hitTemplate, explosive, angleRadian, audioCreationCallback)
        {
            _aimPool = new ObjectPool<AimParticle>(aimTemplate);
            _type = type;
            _distanceBetween = distanceBetweenExplosions;
            _clusterCount = clusterExplosionsCount;
        }

        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward)
        {
            switch (_type)
            {
                case ProjectileTypes.Standard:
                    Create(position).Move(direction, forward, CreateExplosion, CreateAim(targetPosition));
                    break;

                case ProjectileTypes.Cluster:
                    Create(position).Move(
                        direction,
                        forward,
                        explosionPosition => ExplodeCluster(explosionPosition, _clusterCount, _distanceBetween),
                        CreateAim(targetPosition));
                    break;

                case ProjectileTypes.Triple:
                    CreateTriple(position, targetPosition, direction, forward, _distanceBetween, CreateAim);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IAimParticle CreateAim(Vector3 targetPosition)
        {
            return _aimPool.Pull(targetPosition);
        }
    }
}