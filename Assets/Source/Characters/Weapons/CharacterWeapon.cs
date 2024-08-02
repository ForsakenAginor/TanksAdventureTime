using UnityEngine;

namespace Characters
{
    public abstract class CharacterWeapon<TI> : IWeapon, ISwitchable<TI>
        where TI : IDamageableTarget
    {
        public CharacterWeapon(Transform viewPoint, TI target)
        {
            ViewPoint = viewPoint;
            Target = target;
        }

        public Transform ViewPoint { get; }

        public TI Target { get; private set; }

        public void Shoot()
        {
            if (Target == null)
                return;

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