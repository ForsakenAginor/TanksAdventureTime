using UnityEngine;

namespace Enemies
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            TargetTest target,
            Transform transform,
            float attackRadius,
            float rotationSpeed)
            : base(machine, animation, target, transform, attackRadius, rotationSpeed)
        {
        }

        public override void Enter()
        {
            PlayAnimation(EnemyAnimations.Fire);
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            Rotate();

            if (Vector3.Distance(Position, TargetPosition) > AttackRadius)
                SetState<EnemyIdleState>();
        }
    }
}