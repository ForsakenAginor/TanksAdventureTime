namespace Characters
{
    public class SingleAttackState : CharacterAttackState
    {
        private readonly AudioPitcher _sound;

        public SingleAttackState(
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

        public override void OnUpdated()
        {
            _sound.Play();
        }
    }
}