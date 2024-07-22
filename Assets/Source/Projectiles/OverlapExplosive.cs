using UnityEngine;

namespace Projectiles
{
    public class OverlapExplosive : IExplosive
    {
        private readonly LayerMask _mask;
        private readonly Collider[] _colliders;

        public OverlapExplosive(LayerMask mask, int bufferSize = 100)
        {
            _mask = mask;
            _colliders = new Collider[bufferSize];
        }

        public void Explode(Vector3 position, float radius)
        {
            Physics.OverlapSphereNonAlloc(position, radius, _colliders, _mask);

            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i] == null)
                    continue;

                if (_colliders[i].TryGetComponent(out IReactive reactive) == false)
                {
                    _colliders[i] = null;
                    continue;
                }

                reactive.React();
                _colliders[i] = null;
            }
        }
    }
}