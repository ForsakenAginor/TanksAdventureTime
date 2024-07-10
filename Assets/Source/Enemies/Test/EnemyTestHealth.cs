using System;

namespace Enemies
{
    public class EnemyTestHealth : IDamageable
    {
        private const string ExceptionMessage = "The damage must be greater than 0";

        private readonly float MinHealth;

        private float _health;

        public EnemyTestHealth(float maxHealth)
        {
            MinHealth = (float)ValueConstants.Zero;
            _health = maxHealth;
        }

        public event Action Died;

        public event Action<int> DamageTook;

        public void TakeDamage(int value)
        {
            if (value <= MinHealth)
                throw new ArgumentOutOfRangeException(ExceptionMessage);

            _health -= value;
            DamageTook?.Invoke(value);

            if (_health > MinHealth)
                return;

            Died?.Invoke();
        }
    }
}