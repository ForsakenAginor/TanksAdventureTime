using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider))]
    public class TargetTest : MonoBehaviour, IPlayerTarget
    {
        [SerializeField] private Transform _transform;

        private Collider _collider;

        public Vector3 Position => _transform.position;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            print("Target Hit");
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }

        public void TakeHit(HitTypes type)
        {
        }
    }
}