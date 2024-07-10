using System;

public interface IDamageable
{
    public event Action Died;

    public event Action<int> DamageTook;

    public void TakeDamage(int value);
}