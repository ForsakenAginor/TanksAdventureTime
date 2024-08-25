using System;
using Characters;

namespace PlayerHelpers
{
    public class PlayerHelperPresenter
    {
        private readonly FiniteStateMachine<CharacterState> _machine;
        private readonly CharacterThinker _thinker;
        private readonly TargetSwitcher _switcher;
        private readonly (Action onEnable, Action onDisable) _activationHandler;

        public PlayerHelperPresenter(
            FiniteStateMachine<CharacterState> machine,
            CharacterThinker thinker,
            TargetSwitcher switcher,
            (Action onEnable, Action onDisable) activationHandler)
        {
            _machine = machine;
            _thinker = thinker;
            _switcher = switcher;
            _activationHandler = activationHandler;
        }

        public void Enable()
        {
            _thinker.Updated += OnUpdated;

            _activationHandler.onEnable?.Invoke();
        }

        public void Disable()
        {
            _thinker.Updated -= OnUpdated;

            _activationHandler.onDisable?.Invoke();
            _machine.Exit();
        }

        private void OnUpdated()
        {
            _switcher.Search();
            _machine.Update();
        }
    }
}