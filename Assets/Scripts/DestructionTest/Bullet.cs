using UnityEngine;

namespace DestructionTest
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 300f;

        private Rigidbody _rigidbody;

        private void Start() => _rigidbody = GetComponent<Rigidbody>();

        private void FixedUpdate() => _rigidbody.velocity += Vector3.forward * (_speed * Time.deltaTime);

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Destruction destruction))
                destruction.DestroyObject();
        }
    }
}