namespace Enemies
{
    public class EnemyPresenter
    {
        private readonly FiniteStateMachine<EnemyState> Machine;
        private readonly EnemyThinker Thinker;
        private readonly EnemyCollision Collision;
        private readonly IDamageable Health;
        private readonly HitConfiguration HitConfiguration;
        private readonly EnemyDeathEffect Death;

        public EnemyPresenter(
            FiniteStateMachine<EnemyState> machine,
            EnemyThinker thinker,
            EnemyCollision collision,
            IDamageable health,
            HitConfiguration hitConfiguration,
            EnemyDeathEffect death)
        {
            Machine = machine;
            Thinker = thinker;
            Collision = collision;
            Health = health;
            HitConfiguration = hitConfiguration;
            Death = death;
        }

        public void Enable()
        {
            Thinker.Updated += OnUpdated;
            Collision.HitTook += OnHitTook;
            Health.Died += OnDied;
            Health.DamageTook += OnDamageTook;

            Thinker.Start();
        }

        public void Disable()
        {
            Thinker.Updated -= OnUpdated;
            Collision.HitTook -= OnHitTook;
            Health.Died -= OnDied;
            Health.DamageTook -= OnDamageTook;

            OnDisable();
        }

        private void OnUpdated()
        {
            Machine.Update();
        }

        private void OnHitTook(HitTypes type)
        {
            Health.TakeDamage(HitConfiguration.GetDamage(type));
        }

        private void OnDied()
        {
            OnDisable();
            Death.Die();
        }

        private void OnDamageTook(int value)
        {
        }

        private void OnDisable()
        {
            Thinker.Stop();
            Machine.Exit();
        }
    }
}