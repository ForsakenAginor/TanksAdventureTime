namespace Characters
{
    public class CharacterState : FiniteStateMachineState<CharacterState>
    {
        private readonly CharacterAnimation _animation;

        public CharacterState(
            FiniteStateMachine<CharacterState> machine,
            CharacterAnimation animation,
            IFieldOfView fieldOfView)
            : base(machine)
        {
            _animation = animation;
            FieldOfView = fieldOfView;
        }

        public IFieldOfView FieldOfView { get; }

        public void PlayAnimation(CharacterAnimations animation)
        {
            _animation.Play(animation);
        }
    }
}