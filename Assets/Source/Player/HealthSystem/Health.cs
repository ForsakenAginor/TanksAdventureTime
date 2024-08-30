using System;

namespace Player
{
    public class Health
    {
        public Health(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Maximum = amount;
            Current = amount;
        }

        public event Action<int, int> HealthChanged;

        public event Action Died;

        public int Current { get; private set; }

        public int Maximum { get; }

        public void Restore(int health)
        {
            if (health <= 0)
                throw new ArgumentOutOfRangeException(nameof(health));

            Current += health;
            Current = Current >= Maximum ? Maximum : health;
            HealthChanged?.Invoke(Current, Maximum);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Current -= amount;
            Current = Current > 0 ? Current : 0;
            HealthChanged?.Invoke(Current, Maximum);

            if (Current == 0)
                Died?.Invoke();
        }
    }
}