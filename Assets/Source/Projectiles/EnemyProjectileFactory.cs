using System;
using UnityEngine;

namespace Projectiles
{
    public class EnemyProjectileFactory : ProjectileFactory, IProjectileFactory
    {
        private readonly ObjectPool<AimParticle> AimPool;
        private readonly ProjectileTypes Type;
        private readonly float DistanceBetween;
        private readonly int ClusterCount;

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
            AimPool = new ObjectPool<AimParticle>(aimTemplate);
            Type = type;
            DistanceBetween = distanceBetweenExplosions;
            ClusterCount = clusterExplosionsCount;
        }

        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward)
        {
            switch (Type)
            {
                case ProjectileTypes.Standard:
                    Create(position).Move(direction, forward, CreateExplosion, CreateAim(targetPosition));
                    break;

                case ProjectileTypes.Cluster:
                    Create(position).Move(
                        direction,
                        forward,
                        explosionPosition => ExplodeCluster(explosionPosition, ClusterCount, DistanceBetween),
                        CreateAim(targetPosition));
                    break;

                case ProjectileTypes.Triple:
                    CreateTriple(position, targetPosition, direction, forward, DistanceBetween, CreateAim);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IAimParticle CreateAim(Vector3 targetPosition)
        {
            return AimPool.Pull(targetPosition);
        }
    }
}