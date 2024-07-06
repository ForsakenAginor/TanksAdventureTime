using UnityEngine;

namespace Enemies
{
    public class EnemyState : FiniteStateMachineState<EnemyState>
    {
        private readonly EnemyAnimation Animation;

        public EnemyState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            IFieldOfView fieldOfView)
            : base(machine)
        {
            Animation = animation;
            FieldOfView = fieldOfView;
        }

        public IFieldOfView FieldOfView { get; }

        public void PlayAnimation(EnemyAnimations animation)
        {
            Animation.Play(animation);
        }
    }
}