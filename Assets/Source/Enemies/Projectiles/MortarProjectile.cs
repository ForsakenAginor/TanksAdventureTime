using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class MortarProjectile : SpawnableObject
    {
        [SerializeField] private float _radius;
        [SerializeField] private ParticleSystem _explosion;

        private Rigidbody _rigidbody;
        private float _gravity;

        public float Radius => _radius;

        private void OnCollisionEnter(Collision collision)
        {
            Hide();
        }

        public override void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _gravity = Physics.gravity.y;
        }

        public void Move(float angleRadian, Vector3 direction, Vector3 forward)
        {
            Transform.rotation = Quaternion.LookRotation(direction);
            _rigidbody.velocity = forward.CalculateVelocity(direction, angleRadian, _gravity);
        }

        public void Hide()
        {
            _explosion.Play();
            Push();
            _rigidbody.velocity = Vector3.zero;
        }
    }
}