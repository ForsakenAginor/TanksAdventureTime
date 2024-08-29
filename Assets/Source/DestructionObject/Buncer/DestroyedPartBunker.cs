using UnityEngine;

namespace DestructionObject.Buncer
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestroyedPartBunker : MonoBehaviour
    {
        private const string DieObject = nameof(DisableObject);

        private Rigidbody _rigidbody;
        private float _force = 1000f;
        private float _radius = 10;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        public void React(Transform centerObjectPosition)
        {
            AddForce(centerObjectPosition);
            Die();
        }

        private void AddForce(Transform centerObjectPosition) => _rigidbody.AddExplosionForce(_force, centerObjectPosition.position, _radius);

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