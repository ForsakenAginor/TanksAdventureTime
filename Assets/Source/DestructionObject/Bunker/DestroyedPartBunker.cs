using UnityEngine;

namespace DestructionObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestroyedPartBunker : MonoBehaviour
    {
        private const string DieObject = nameof(DisableObject);
        private const float Force = 1000f;
        private const int Radius = 10;

        private Rigidbody _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        public void React(Transform centerObjectPosition)
        {
            AddForce(centerObjectPosition);
            Die();
        }

        private void AddForce(Transform centerObjectPosition) =>
            _rigidbody.AddExplosionForce(Force, centerObjectPosition.position, Radius);

        private void Die()
        {
            float minValue = 5f;
            float maxValue = 10f;
            float timeDie = Random.Range(minValue, maxValue);
            Invoke(DieObject, timeDie);
        }

        private void DisableObject() => gameObject.SetActive(false);
    }
}