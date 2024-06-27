using System;
using UnityEngine;

namespace Enemies
{
    public class WeaponFactory : IEnemyFactory<IWeapon>
    {
        private readonly Transform ViewPoint;
        private readonly IPlayerTarget Target;
        private readonly HitEffect HitEffect;
        private readonly ParticleSystem ShootingEffect;
        private readonly AudioSource Sound;
        private readonly MortarProjectile Projectile;
        private readonly float Angle;

        public WeaponFactory(
            Transform viewPoint,
            IPlayerTarget target,
            HitEffect hitEffect,
            ParticleSystem shootingEffect,
            AudioSource sound,
            MortarProjectile projectile,
            float attackAngle)
        {
            ViewPoint = viewPoint;
            Target = target;
            HitEffect = hitEffect;
            ShootingEffect = shootingEffect;
            Sound = sound;
            Projectile = projectile;
            Angle = attackAngle;
        }

        public IWeapon Create(EnemyTypes type)
        {
            switch (type)
            {
                case EnemyTypes.Standard:
                case EnemyTypes.Bunker:
                    return new Gun(HitEffect, ViewPoint, Target, ShootingEffect, Sound);

                case EnemyTypes.Mortar:
                    return new Mortar(Projectile, ViewPoint, Target, Angle, Sound);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}