using UnityEngine;

namespace Enemies
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(
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
            PlayAnimation(EnemyAnimations.Idle);
        }

        public override void Update()
        {
            if (Vector3.Distance(Position, TargetPosition) > AttackRadius)
                return;

            SetState<EnemyAttackState>();
        }
    }
}