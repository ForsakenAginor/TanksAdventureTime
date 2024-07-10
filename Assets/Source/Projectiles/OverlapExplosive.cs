using UnityEngine;

namespace Projectiles
{
    public class OverlapExplosive : IExplosive
    {
        private readonly LayerMask Mask;
        private readonly Collider[] Colliders;

        public OverlapExplosive(LayerMask mask, int bufferSize = 100)
        {
            Mask = mask;
            Colliders = new Collider[bufferSize];
        }

        public void Explode(Vector3 position, float radius)
        {
            Physics.OverlapSphereNonAlloc(position, radius, Colliders, Mask);

            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i] == null)
                    continue;

                if (Colliders[i].TryGetComponent(out IReactive reactive) == false)
                {
                    Colliders[i] = null;
                    continue;
                }

                reactive.React();
                Colliders[i] = null;
            }
        }
    }
}