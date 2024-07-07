using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ObjectPool<SpawnableProjectile> Weapon;
        private readonly ObjectPool<HitEffect> ExplosionPool;
        private readonly ObjectPool<AimParticle> AimPool;
        private readonly IDamageableTarget Target;
        private readonly ProjectileTypes Type;
        private readonly float AngleRadian;
        private readonly float DistanceBetween;
        private readonly int ClusterCount;

        public ProjectileFactory(
            SpawnableProjectile projectile,
            HitEffect hitTemplate,
            AimParticle aimTemplate,
            IDamageableTarget target,
            ProjectileTypes type,
            float angleRadian,
            float distanceBetween,
            int clusterCount)
        {
            Weapon = new ObjectPool<SpawnableProjectile>(projectile);
            ExplosionPool = new ObjectPool<HitEffect>(hitTemplate);
            AimPool = new ObjectPool<AimParticle>(aimTemplate);
            Target = target;
            Type = type;
            AngleRadian = angleRadian;
            DistanceBetween = distanceBetween;
            ClusterCount = clusterCount;
        }

        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward)
        {
            IExplosive explosive = new Explosive(Target);

            switch (Type)
            {
                case ProjectileTypes.Standard:
                    Create(position, explosive).Move(direction, forward, CreateExplosion, CreateAim(targetPosition));
                    break;

                case ProjectileTypes.Cluster:
                    Create(position, explosive).Move(direction, forward, ExplodeCluster, CreateAim(targetPosition));
                    break;

                case ProjectileTypes.Triple:
                    CreateTriple(position, targetPosition, direction, forward, explosive);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SpawnableProjectile Create(Vector3 position, IExplosive explosive)
        {
            return Weapon.Pull(position).Init(explosive, AngleRadian);
        }

        private void CreateTriple(
            Vector3 position,
            Vector3 targetPosition,
            Vector3 direction,
            Vector3 forward,
            IExplosive explosive)
        {
            Vector3 cross = Vector3.Cross(direction, Vector3.up).normalized * DistanceBetween;
            Vector3 leftTarget = cross + targetPosition;
            Vector3 rightTarget = -cross + targetPosition;
            Vector3 leftDirection = leftTarget - position;
            Vector3 rightDirection = -cross + targetPosition - position;
            Vector3 leftForward = forward.RotateAlongY(leftDirection);
            Vector3 rightForward = forward.RotateAlongY(rightDirection);
            Create(position, explosive).Move(direction, forward, CreateExplosion, CreateAim(targetPosition));
            Create(position, explosive).Move(leftDirection, leftForward, CreateExplosion, CreateAim(leftTarget));
            Create(position, explosive).Move(rightDirection, rightForward, CreateExplosion, CreateAim(rightTarget));
        }

        private void CreateExplosion(Vector3 position)
        {
            ExplosionPool.Pull(position).Init();
        }

        private void ExplodeCluster(Vector3 position)
        {
            CreateExplosion(position);
            position += Vector3.up;

            for (int i = 0; i < ClusterCount; i++)
            {
                Vector3 newPosition = new Vector3(
                    Random.Range(-DistanceBetween, DistanceBetween) + position.x,
                    position.y,
                    Random.Range(-DistanceBetween, DistanceBetween) + position.z);
                Vector3 direction = newPosition - position;
                Vector3 forward = new Vector3(direction.x, (float)ValueConstants.Zero, direction.z).normalized;
                forward.y = newPosition.y;
                forward /= Mathf.Cos(AngleRadian);
                Create(position, new Explosive(Target)).Move(direction, forward, CreateExplosion);
            }
        }

        private IAimParticle CreateAim(Vector3 targetPosition)
        {
            return AimPool.Pull(targetPosition);
        }
    }
}