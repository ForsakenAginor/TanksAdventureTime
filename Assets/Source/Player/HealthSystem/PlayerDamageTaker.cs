using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider))]
    public class PlayerDamageTaker : MonoBehaviour, IPlayerTarget, IPermanentKiller
    {
        private const int BulletDamage = 1;
        private const int ExplosionDamage = 10;

        private VirtualCameraShaker _shaker;
        private Health _health;
        private Transform _transform;
        private Collider _collider;
        private bool _isWorking;

        public event Action PlayerDied;

        public Vector3 Position => _transform.position;

        public TargetPriority Priority { get; private set; } = TargetPriority.High;

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
            _health = health ?? throw new ArgumentNullException(nameof(health));
            _shaker = shaker ?? throw new ArgumentNullException(nameof(shaker));
            _isWorking = true;
            _health.Died += OnDied;
        }

        public Vector3 GetClosestPoint(Vector3 position) => _collider.ClosestPoint(position);

        public void TakeHit(HitTypes type)
        {
            if (_isWorking == false)
                return;

            switch (type)
            {
                case HitTypes.Bullet:
                    _health.TakeDamage(BulletDamage);
                    break;

                case HitTypes.Explosion:
                    TakeExplosiveDamage();
                    break;

                case HitTypes.PermanentDeath:
                case HitTypes.PlayerExplosion:
                default:
                    break;
            }
        }

        public void Continue()
        {
            _isWorking = true;
            _health.Restore(_health.Maximum);
            Priority = TargetPriority.High;
        }

        public void StopWorking()
        {
            Priority = TargetPriority.None;
            _isWorking = false;
        }

        private void OnDied()
        {
            PlayerDied?.Invoke();
        }

        private void TakeExplosiveDamage()
        {
            _shaker.Shake();
            _health.TakeDamage(ExplosionDamage);
        }
    }
}