using UnityEngine;

namespace Enemies
{
    public class Gun : EnemyWeapon
    {
        private readonly ParticleSystem ShootingEffect;

        public Gun(
            HitEffect hitEffect,
            Transform viewPoint,
            IPlayerDetector target,
            ParticleSystem shootingEffect,
            AudioSource sound)
            : base(hitEffect, viewPoint, target, sound)
        {
            ShootingEffect = shootingEffect;
        }

        public override void OnShoot()
        {
            ShootingEffect.Play();
            Vector3 closest = Target.GetClosestPoint(ViewPoint.position);
            Pull<HitEffect>(closest).Init(ViewPoint.position);
            Target.TakeHit(HitTypes.Bullet);
        }
    }
}