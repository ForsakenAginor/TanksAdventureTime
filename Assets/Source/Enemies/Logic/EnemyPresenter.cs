namespace Enemies
{
    public class EnemyPresenter
    {
        private readonly FiniteStateMachine<EnemyState> _machine;
        private readonly EnemyThinker _thinker;
        private readonly EnemyCollision _collision;
        private readonly IDamageable _health;
        private readonly HitConfiguration _hitConfiguration;
        private readonly EnemyDeathEffect _death;

        public EnemyPresenter(
            FiniteStateMachine<EnemyState> machine,
            EnemyThinker thinker,
            EnemyCollision collision,
            IDamageable health,
            HitConfiguration hitConfiguration,
            EnemyDeathEffect death)
        {
            _machine = machine;
            _thinker = thinker;
            _collision = collision;
            _health = health;
            _hitConfiguration = hitConfiguration;
            _death = death;
        }

        public void Enable()
        {
            _thinker.Updated += OnUpdated;
            _collision.HitTook += OnHitTook;
            _health.Died += OnDied;

            _thinker.Start();
        }

        public void Disable()
        {
            _thinker.Updated -= OnUpdated;
            _collision.HitTook -= OnHitTook;
            _health.Died -= OnDied;

            OnDisable();
        }

        private void OnUpdated()
        {
            _machine.Update();
        }

        private void OnHitTook(HitTypes type)
        {
            _health.TakeDamage(_hitConfiguration.GetDamage(type));
        }

        private void OnDied()
        {
            OnDisable();
            _death.Die();
        }

        private void OnDisable()
        {
            _thinker.Stop();
            _machine.Exit();
        }
    }
}