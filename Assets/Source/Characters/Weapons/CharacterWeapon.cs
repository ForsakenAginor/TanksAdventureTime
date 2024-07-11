using UnityEngine;

namespace Characters
{
    public abstract class CharacterWeapon<TI> : IWeapon
        where TI: IDamageableTarget
    {
        private readonly AudioPitcher _sound;

        public CharacterWeapon(Transform viewPoint, TI target, AudioPitcher sound)
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