namespace Enemies
{
    public class EnemyAttackState : EnemyState
    {
        private readonly EnemyRotator Rotator;
        private readonly IWeapon Weapon;

        public EnemyAttackState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            IFieldOfView fieldOfView,
            EnemyRotator rotator,
            IWeapon weapon)
            : base(machine, animation, fieldOfView)
        {
            Rotator = rotator;
            Weapon = weapon;
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
            Weapon.Shoot();

            if (FieldOfView.CanView() == false || FieldOfView.IsBlockingByWall() == true)
                SetState<EnemyIdleState>();
        }
    }
}