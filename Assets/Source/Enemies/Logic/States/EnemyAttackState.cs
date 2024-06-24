namespace Enemies
{
    public class EnemyAttackState : EnemyState
    {
        private readonly EnemyRotator Rotator;

        public EnemyAttackState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            IFieldOfView fieldOfView,
            EnemyRotator rotator)
            : base(machine, animation, fieldOfView)
        {
            Rotator = rotator;
        }

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Fire);
            Rotator.StartRotation();
        }

        public override void Exit()
        {
            Rotator.StopRotation();
        }

        public override void Update()
        {
            if (FieldOfView.IsPlayerInRadius() == false || FieldOfView.IsBlockingByWall() == true)
                SetState<EnemyIdleState>();
        }
    }
}