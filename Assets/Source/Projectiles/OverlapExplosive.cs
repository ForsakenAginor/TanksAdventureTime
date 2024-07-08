using UnityEngine;

namespace Projectiles
{
    public class OverlapExplosive : IExplosive
    {
        private readonly LayerMask _mask;
        
        public void Explode(Vector3 position, float radius)
        {
            Physics.OverlapSphere(position, radius);
        }
    }
}