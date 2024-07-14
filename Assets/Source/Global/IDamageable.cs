using System;

public interface IDamageable
{
    public event Action Died;

    public void TakeDamage(int value);
}