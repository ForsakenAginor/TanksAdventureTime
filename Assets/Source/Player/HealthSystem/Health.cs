using System;

namespace Assets.Source.Player.HealthSystem
{
    public class Health
    {
        private readonly int _maximum;
        private int _current;

        public Health(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _maximum = amount;
            _current = amount;
        }

        public event Action<int, int> HealthChanged;

        public event Action Died;

        public int Current => _current;

        public int Maximum => _maximum;

        public void Restore(int health)
        {
            if (health <= 0)
                throw new ArgumentOutOfRangeException(nameof(health));

            _current += health;
            _current = _current >= _maximum ? _maximum : health;
            HealthChanged?.Invoke(_current, _maximum);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _current -= amount;
            _current = _current > 0 ? _current : 0;
            HealthChanged?.Invoke(_current, _maximum);

            if (_current == 0)
                Died?.Invoke();
        }
    }
}