using System;

namespace Enemies
{
    public class EnemyHealth : IDamageable
    {
        private const string ExceptionMessage = "The damage must be greater or equals 0";

        private readonly int _minHealth;

        private int _health;

        public EnemyHealth(int maxHealth)
        {
            _minHealth = (int)ValueConstants.Zero;
            _health = maxHealth;
        }

        public event Action Died;

        public void TakeDamage(int value)
        {
            if (value < _minHealth)
                throw new ArgumentOutOfRangeException(ExceptionMessage);

            if (value == _minHealth)
                return;

            _health -= value;

            if (_health > _minHealth)
                return;

            Died?.Invoke();
        }
    }
}