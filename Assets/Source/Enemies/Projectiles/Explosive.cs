using UnityEngine;

namespace Enemies
{
    public class Explosive : IExplosive
    {
        private readonly IDamageableTarget Target;

        public Explosive(IDamageableTarget target)
        {
            Target = target;
        }

        public void Explode(Vector3 position, float radius)
        {
            if (Vector3.Distance(position, Target.Position) > radius)
                return;

            Target.TakeHit(HitTypes.Explosion);
        }
    }
}