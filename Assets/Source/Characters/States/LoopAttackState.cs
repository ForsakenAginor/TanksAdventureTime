namespace Characters
{
    public class LoopAttackState : CharacterAttackState
    {
        private readonly AudioPitcher _sound;

        public LoopAttackState(
            FiniteStateMachine<CharacterState> machine,
            IFieldOfView fieldOfView,
            CharacterRotator rotator,
            IWeapon weapon,
            AudioPitcher sound,
            CharacterAnimation animation = null)
            : base(machine, fieldOfView, rotator, weapon, animation)
        {
            _sound = sound;
        }

        public override void OnEntered()
        {
            _sound.Play();
        }

        public override void OnExited()
        {
            _sound.Stop();
        }
    }
}