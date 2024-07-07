﻿using UnityEngine;

namespace Enemies
{
    public abstract class EnemyWeapon<TI> : IWeapon
        where TI: IDamageableTarget
    {
        private readonly AudioPitcher Sound;

        public EnemyWeapon(Transform viewPoint, TI target, AudioPitcher sound)
        {
            ViewPoint = viewPoint;
            Target = target;
            Sound = sound;
        }

        public Transform ViewPoint { get; }

        public TI Target { get; }

        public void Shoot()
        {
            Sound.Play();
            OnShoot();
        }

        public virtual void OnShoot()
        {
        }
    }
}