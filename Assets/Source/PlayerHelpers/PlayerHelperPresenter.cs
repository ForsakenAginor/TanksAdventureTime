using Characters;

namespace PlayerHelpers
{
    public class PlayerHelperPresenter
    {
        private readonly FiniteStateMachine<CharacterState> _machine;
        private readonly CharacterThinker _thinker;
        private readonly TargetSwitcher _switcher;

        public PlayerHelperPresenter(
            FiniteStateMachine<CharacterState> machine,
            CharacterThinker thinker,
            TargetSwitcher switcher)
        {
            _machine = machine;
            _thinker = thinker;
            _switcher = switcher;
        }

        public void Enable()
        {
            _thinker.Updated += OnUpdated;

            _switcher.StartSearching();
            _thinker.Start();
        }

        public void Disable()
        {
            _thinker.Updated -= OnUpdated;

            _switcher.StopSearching();
            _thinker.Stop();
            _machine.Exit();
        }

        private void OnUpdated()
        {
            _machine.Update();
        }
    }
}