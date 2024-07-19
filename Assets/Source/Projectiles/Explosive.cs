using Characters;
using UnityEngine;

namespace Projectiles
{
    public class Explosive : IExplosive, ISwitchable<IDamageableTarget>
    {
        private IDamageableTarget _target;

        public Explosive(IDamageableTarget target)
        {
            _target = target;
        }

        public void Explode(Vector3 position, float radius)
        {
            if (_target == null)
                return;

            if (Vector3.Distance(position, _target.Position) > radius)
                return;

            _target.TakeHit(HitTypes.Explosion);
        }

        public void Switch(IDamageableTarget target)
        {
            _target = target;
        }
    }
}