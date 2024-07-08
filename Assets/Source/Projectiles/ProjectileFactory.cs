using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public class ProjectileFactory
    {
        private readonly ObjectPool<SpawnableProjectile> Weapon;
        private readonly ObjectPool<HitEffect> ExplosionPool;
        private readonly IExplosive Explosive;
        private readonly float AngleRadian;

        public ProjectileFactory(
            SpawnableProjectile projectile,
            HitEffect hitTemplate,
            IExplosive explosive,
            float angleRadian)
        {
            Weapon = new ObjectPool<SpawnableProjectile>(projectile);
            ExplosionPool = new ObjectPool<HitEffect>(hitTemplate);
            Explosive = explosive;
            AngleRadian = angleRadian;
        }

        public SpawnableProjectile Create(Vector3 position)
        {
            return Weapon.Pull(position).Init(Explosive, AngleRadian);
        }

        public void CreateTriple(
            Vector3 position,
            Vector3 targetPosition,
            Vector3 direction,
            Vector3 forward,
            float distanceBetween,
            Func<Vector3, IAimParticle> aimCreateCallback = null)
        {
            Vector3 cross = Vector3.Cross(direction, Vector3.up).normalized * distanceBetween;
            Vector3 leftTarget = cross + targetPosition;
            Vector3 rightTarget = -cross + targetPosition;
            Vector3 leftDirection = leftTarget - position;
            Vector3 rightDirection = -cross + targetPosition - position;
            Vector3 leftForward = forward.RotateAlongY(leftDirection);
            Vector3 rightForward = forward.RotateAlongY(rightDirection);
            Create(position).Move(
                direction,
                forward,
                CreateExplosion,
                aimCreateCallback?.Invoke(targetPosition));
            Create(position).Move(
                leftDirection,
                leftForward,
                CreateExplosion,
                aimCreateCallback?.Invoke(leftTarget));
            Create(position).Move(
                rightDirection,
                rightForward,
                CreateExplosion,
                aimCreateCallback?.Invoke(rightTarget));
        }

        public void ExplodeCluster(Vector3 position, float clusterCount, float distanceBetween)
        {
            CreateExplosion(position);
            position += Vector3.up;

            for (int i = 0; i < clusterCount; i++)
            {
                Vector3 newPosition = new Vector3(
                    Random.Range(-distanceBetween, distanceBetween) + position.x,
                    position.y,
                    Random.Range(-distanceBetween, distanceBetween) + position.z);
                Vector3 direction = newPosition - position;
                Vector3 forward = new Vector3(direction.x, (float)ValueConstants.Zero, direction.z).normalized;
                forward.y = newPosition.y;
                forward /= Mathf.Cos(AngleRadian);
                Create(position).Move(direction, forward, CreateExplosion);
            }
        }

        public void CreateExplosion(Vector3 position)
        {
            ExplosionPool.Pull(position).Init();
        }
    }
}