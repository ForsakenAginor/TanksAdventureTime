namespace Enemies
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            IFieldOfView fieldOfView)
            : base(machine, animation, fieldOfView)
        {
        }

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Idle);
        }

        public override void Update()
        {
            if (FieldOfView.IsPlayerInRadius() == false)
                return;

            if (FieldOfView.IsBlockingByWall() == true)
                return;

            SetState<EnemyAttackState>();
        }
    }
}