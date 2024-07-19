using System;
using UnityEngine;

namespace Assets.Source.Player.HealthSystem
{
    [RequireComponent(typeof(Collider))]
    public class PlayerDamageTaker : MonoBehaviour, IPlayerTarget, IPermanentKiller
    {
        private Health _health;
        private Transform _transform;
        private Collider _collider;

        public event Action PlayerDied;

        public Vector3 GetClosestPoint(Vector3 position) => _collider.ClosestPoint(position);

        public void TakeHit(HitTypes type)
        {
            if (type != HitTypes.Explosion)
                return;
        }

        public Vector3 Position => _transform.position;

        public TargetPriority Priority => throw new System.NotImplementedException();

        private void Awake()
        {
            _transform = transform;
            _collider = GetComponent<Collider>();
        }

        private void OnDestroy()
        {
            _health.Died -= OnDied;
        }

        public void Init(Health health)
        {
            _health = health != null ? health : throw new ArgumentNullException(nameof(health));
            _health.Died += OnDied;
        }

        private void OnDied()
        {
            PlayerDied?.Invoke();
        }

        public void TestTakeDamage(int amount)
        {
            _health.TakeDamage(amount);
        }

        public void Respawn()
        {
            _health.Restore(_health.Maximum);
        }
    }
}