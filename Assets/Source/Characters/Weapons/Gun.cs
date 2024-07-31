using System;
using Projectiles;
using UnityEngine;

namespace Characters
{
    public class Gun : CharacterWeapon<IPlayerTarget>
    {
        private readonly ParticleSystem _shootingEffect;
        private readonly ObjectPool<HitEffect> _hitPool;
        private readonly Action<AudioSource> _audioCreationCallback;

        public Gun(
            HitEffect hitEffect,
            Transform viewPoint,
            IPlayerTarget target,
            ParticleSystem shootingEffect,
            Action<AudioSource> audioCreationCallback)
            : base(viewPoint, target)
        {
            _hitPool = new ObjectPool<HitEffect>(hitEffect);
            _shootingEffect = shootingEffect;
            _audioCreationCallback = audioCreationCallback;
        }

        public override void OnShoot()
        {
            _shootingEffect.Play();
            _hitPool.Pull(Target.GetClosestPoint(ViewPoint.position)).Init(ViewPoint.position, _audioCreationCallback);
            Target.TakeHit(HitTypes.Bullet);
        }
    }
}