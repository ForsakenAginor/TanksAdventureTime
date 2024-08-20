using UnityEngine;

namespace Projectiles
{
    public class Explosive : IExplosive
    {
        private readonly IDamageableTarget _target;

        public Explosive(IDamageableTarget target)
        {
            _target = target;
        }

        public void Explode(Vector3 position, float radius)
        {
            if (Vector3.Distance(position, _target.Position) > radius)
                return;

            _target.TakeHit(HitTypes.Explosion);
        }
    }
}