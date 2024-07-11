namespace Enemies
{
    public class EnemyState : FiniteStateMachineState<EnemyState>
    {
        private readonly EnemyAnimation _animation;

        public EnemyState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            IFieldOfView fieldOfView)
            : base(machine)
        {
            _animation = animation;
            FieldOfView = fieldOfView;
        }

        public IFieldOfView FieldOfView { get; }

        public void PlayAnimation(EnemyAnimations animation)
        {
            _animation.Play(animation);
        }
    }
}