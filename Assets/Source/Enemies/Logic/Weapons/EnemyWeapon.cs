using UnityEngine;

namespace Enemies
{
    public abstract class EnemyWeapon<TI> : IWeapon
        where TI: IDamageableTarget
    {
        private readonly AudioPitcher _sound;

        public EnemyWeapon(Transform viewPoint, TI target, AudioPitcher sound)
        {
            ViewPoint = viewPoint;
            Target = target;
            _sound = sound;
        }

        public Transform ViewPoint { get; }

        public TI Target { get; }

        public void Shoot()
        {
            _sound.Play();
            OnShoot();
        }

        public virtual void OnShoot()
        {
        }
    }
}