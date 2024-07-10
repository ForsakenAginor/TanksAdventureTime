using System;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Gun : EnemyWeapon<IPlayerTarget>
    {
        private readonly ParticleSystem ShootingEffect;
        private readonly ObjectPool<HitEffect> HitPool;
        private readonly Action<AudioSource> AudioCreationCallback;

        public Gun(
            HitEffect hitEffect,
            Transform viewPoint,
            IPlayerTarget target,
            ParticleSystem shootingEffect,
            AudioPitcher sound,
            Action<AudioSource> audioCreationCallback)
            : base(viewPoint, target, sound)
        {
            HitPool = new ObjectPool<HitEffect>(hitEffect);
            ShootingEffect = shootingEffect;
            AudioCreationCallback = audioCreationCallback;
        }

        public override void OnShoot()
        {
            ShootingEffect.Play();
            HitPool.Pull(Target.GetClosestPoint(ViewPoint.position)).Init(ViewPoint.position, AudioCreationCallback);
            Target.TakeHit(HitTypes.Bullet);
        }
    }
}