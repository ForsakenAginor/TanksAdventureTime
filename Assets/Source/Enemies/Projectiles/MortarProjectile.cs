using System;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class MortarProjectile : SpawnableObject
    {
        [SerializeField] private float _explosionRadius;

        private Rigidbody _rigidbody;
        private IAimParticle _aim;
        private IExplosive _explosive;
        private Action<Vector3> _onExplodedCallback;
        private float _angleRadian;
        private bool _didInit;

        public float ColliderRadius { get; private set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }

        private void OnCollisionEnter()
        {
            Explode();
        }

        public MortarProjectile Init(IExplosive explosive, float angleRadian)
        {
            if (_didInit == true)
                return this;

            _rigidbody = GetComponent<Rigidbody>();
            ColliderRadius = GetComponent<SphereCollider>().radius;
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

        private void Explode()
        {
            _rigidbody.velocity = Vector3.zero;
            _aim?.Hide();
            _aim = null;
            _onExplodedCallback.Invoke(Transform.position);
            _explosive.Explode(Transform.position, _explosionRadius);
            Push();
        }
    }
}