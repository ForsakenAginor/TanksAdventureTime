namespace Characters
{
    public class CharacterAttackState : CharacterState
    {
        private readonly CharacterRotator _rotator;
        private readonly IWeapon _weapon;

        public CharacterAttackState(
            FiniteStateMachine<CharacterState> machine,
            IFieldOfView fieldOfView,
            CharacterRotator rotator,
            IWeapon weapon,
            CharacterAnimation animation = null)
            : base(machine, animation, fieldOfView)
        {
            _rotator = rotator;
            _weapon = weapon;
        }

        public override void Enter()
        {
            PlayAnimation(CharacterAnimations.Fire);
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
                SetState<CharacterIdleState>();
        }
    }
}