namespace Enemies
{
    public class EnemyAttackState : EnemyState
    {
        private readonly EnemyRotator _rotator;
        private readonly IWeapon _weapon;

        public EnemyAttackState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            IFieldOfView fieldOfView,
            EnemyRotator rotator,
            IWeapon weapon)
            : base(machine, animation, fieldOfView)
        {
            _rotator = rotator;
            _weapon = weapon;
        }

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Fire);
            _rotator.StartRotation();
        }

        public override void Exit()
        {
            _rotator.StopRotation();
        }

        public override void Update()
        {
            _weapon.Shoot();

            if (FieldOfView.CanView() == false || FieldOfView.IsBlockingByWall() == true)
                SetState<EnemyIdleState>();
        }
    }
}