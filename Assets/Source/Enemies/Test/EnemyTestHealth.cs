using System;

namespace Enemies
{
    public class EnemyTestHealth : IDamageable // Используется только в EnemyTesting сцене
    {
        private const string ExceptionMessage = "The damage must be greater than 0";

        private readonly float _minHealth;

        private float _health;

        public EnemyTestHealth(float maxHealth)
        {
            _minHealth = (float)ValueConstants.Zero;
            _health = maxHealth;
        }

        public event Action Died;


        public void TakeDamage(int value)
        {
            if (value <= _minHealth)
                throw new ArgumentOutOfRangeException(ExceptionMessage);

            _health -= value;

            if (_health > _minHealth)
                return;

            Died?.Invoke();
        }
    }
}