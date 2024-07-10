using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Gun : EnemyWeapon<IPlayerTarget>
    {
        private readonly ParticleSystem ShootingEffect;
        private readonly ObjectPool<HitEffect> HitPool;

        public Gun(
            HitEffect hitEffect,
            Transform viewPoint,
            IPlayerTarget target,
            ParticleSystem shootingEffect,
            AudioPitcher sound)
            : base(viewPoint, target, sound)
        {
            HitPool = new ObjectPool<HitEffect>(hitEffect);
            ShootingEffect = shootingEffect;
        }

        public override void OnShoot()
        {
            ShootingEffect.Play();
            HitPool.Pull(Target.GetClosestPoint(ViewPoint.position)).Init(ViewPoint.position);
            Target.TakeHit(HitTypes.Bullet);
        }
    }
}