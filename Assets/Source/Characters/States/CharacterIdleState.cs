namespace Characters
{
    public class CharacterIdleState : CharacterState
    {
        public CharacterIdleState(
            FiniteStateMachine<CharacterState> machine,
            IFieldOfView fieldOfView,
            CharacterAnimation animation = null)
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