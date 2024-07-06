using System;
using UnityEngine;

namespace Assets.Source.Player.HealthSystem
{
    public class PlayerDamageTaker : MonoBehaviour
    {
        private Health _health;

        public event Action PlayerDied;

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
    }
}