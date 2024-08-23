using System;
using UnityEngine;

namespace Assets.Source.Player.HealthSystem
{
    [RequireComponent(typeof(Collider))]
    public class PlayerDamageTaker : MonoBehaviour, IPlayerTarget, IPermanentKiller
    {
        private readonly int _bulletDamage = 1;
        private readonly int _explosionDamage = 10;
        private VirtualCameraShaker _shaker;
        private Health _health;
        private Transform _transform;
        private Collider _collider;
        private bool _isWorking;

        public event Action PlayerDied;

        public Vector3 GetClosestPoint(Vector3 position) => _collider.ClosestPoint(position);

        public void TakeHit(HitTypes type)
        {
            if (_isWorking == false)
                return;

            switch(type)
            {
                case HitTypes.Bullet:
                    _health.TakeDamage(_bulletDamage);
                    break;

                case HitTypes.Explosion:
                    TakeExplosiveDamage();
                    break;

                default:
                    break;
            }
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

        public void Init(Health health, VirtualCameraShaker shaker)
        {
            _health = health != null ? health : throw new ArgumentNullException(nameof(health));
            _shaker = shaker != null ? shaker : throw new ArgumentNullException(nameof(shaker));
            _isWorking = true;
            _health.Died += OnDied;
        }

        public void Continue()
        {
            _isWorking = true;
            _health.Restore(_health.Maximum);
        }

        public void StopWorking()
        {
            _isWorking = false;
        }

        private void OnDied()
        {
            PlayerDied?.Invoke();
        }

        private void TakeExplosiveDamage()
        {
            _shaker.Shake();
            _health.TakeDamage(_explosionDamage);
        }
    }
}