using UnityEngine;

namespace Characters
{
    public abstract class CharacterWeapon<TI> : IWeapon, ISwitchable<TI>
        where TI : IDamageableTarget
    {
        private readonly AudioPitcher _sound;

        public CharacterWeapon(Transform viewPoint, TI target, AudioPitcher sound)
        {
            ViewPoint = viewPoint;
            Target = target;
            _sound = sound;
        }

        public Transform ViewPoint { get; }

        public TI Target { get; private set; }

        public void Shoot()
        {
            _sound.Play();
            OnShoot();
        }

        public void Switch(TI target)
        {
            Target = target;
        }

        public virtual void OnShoot()
        {
        }
    }
}