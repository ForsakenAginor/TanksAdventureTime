using UnityEngine;

namespace Enemies
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _explosionRadius;

        private IExplosive _explosive;
        private Transform _transform;

        private void OnCollisionEnter()
        {
            OnExploding();
        }

        public void Init(IExplosive explosive)
        {
            _explosive = explosive;
            _transform = transform;
        }

        public void Explode()
        {
            _explosive.Explode(_transform.position, _explosionRadius);
        }

        public virtual void OnExploding()
        {
            Explode();
        }
    }
}