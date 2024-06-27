namespace Enemies
{
    public class EnemyPresenter
    {
        private readonly FiniteStateMachine<EnemyState> Machine;
        private readonly EnemyThinker Thinker;

        public EnemyPresenter(FiniteStateMachine<EnemyState> machine, EnemyThinker thinker)
        {
            Machine = machine;
            Thinker = thinker;
        }

        public void Enable()
        {
            Thinker.Updated += OnUpdated;

            Thinker.Start();
        }

        public void Disable()
        {
            Thinker.Updated -= OnUpdated;

            Machine.Exit();
        }

        private void OnUpdated()
        {
            Machine.Update();
        }
    }
}