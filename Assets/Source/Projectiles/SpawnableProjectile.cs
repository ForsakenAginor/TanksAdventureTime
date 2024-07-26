using System;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class SpawnableProjectile : SpawnableObject, ITarget
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private SphereCollider _collider;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private IAimParticle _aim;
        private IExplosive _explosive;
        private Action<Vector3> _onExplodedCallback;
        private float _angleRadian;
        private bool _didInit;

        public float ColliderRadius => _collider.radius;

        public Vector3 Position => _rigidbody.position;

        public TargetPriority Priority => TargetPriority.None;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }

        private void OnCollisionEnter()
        {
            Explode();
        }

        public SpawnableProjectile Init(IExplosive explosive, float angleRadian)
        {
            if (_didInit == true)
                return this;

            _rigidbody = GetComponent<Rigidbody>();
            _explosive = explosive;
            _angleRadian = angleRadian;
            _didInit = true;
            return this;
        }

        public void Move(
            Vector3 direction,
            Vector3 forward,
            Action<Vector3> onExplodedCallback,
            IAimParticle aim = null)
        {
            _onExplodedCallback = onExplodedCallback;
            _aim = aim;
            Transform.rotation = Quaternion.LookRotation(forward);
            _rigidbody.velocity = forward.CalculateVelocity(direction, _angleRadian);
            _aim?.Show();
        }

        public void Explode()
        {
            _rigidbody.velocity = Vector3.zero;
            _aim?.Hide();
            _aim = null;
            _onExplodedCallback?.Invoke(Transform.position);
            _onExplodedCallback = null;
            _explosive.Explode(Transform.position, _explosionRadius);
            Push();
        }
    }
}