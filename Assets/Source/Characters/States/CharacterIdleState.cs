namespace Characters
{
    public class CharacterIdleState : CharacterState
    {
        public CharacterIdleState(
            FiniteStateMachine<CharacterState> machine,
            CharacterAnimation animation,
            IFieldOfView fieldOfView)
            : base(machine, animation, fieldOfView)
        {
        }

        public override void Enter()
        {
            PlayAnimation(CharacterAnimations.Idle);
        }

        public override void Update()
        {
            if (FieldOfView.CanView() == false)
                return;

            if (FieldOfView.IsBlockingByWall() == true)
                return;

            SetState<CharacterAttackState>();
        }
    }
}