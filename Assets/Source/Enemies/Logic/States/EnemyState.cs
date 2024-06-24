using UnityEngine;

namespace Enemies
{
    public class EnemyState : FiniteStateMachineState<EnemyState>
    {
        private readonly EnemyAnimation Animation;
        private readonly TargetTest Target;
        private readonly Transform Transform;
        private readonly float RotationSpeed;

        public EnemyState(
            FiniteStateMachine<EnemyState> machine,
            EnemyAnimation animation,
            TargetTest target,
            Transform transform,
            float attackRadius,
            float rotationSpeed)
            : base(machine)
        {
            Animation = animation;
            Target = target;
            Transform = transform;
            AttackRadius = attackRadius;
            RotationSpeed = rotationSpeed;
        }

        public Vector3 Position => Transform.position;

        public Vector3 TargetPosition => Target.Position;

        public float AttackRadius { get; }

        public void PlayAnimation(EnemyAnimations animation)
        {
            Animation.Play(animation);
        }

        public void Rotate()
        {
            Vector3 direction = (TargetPosition - Position).normalized;
            Quaternion look = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, look, RotationSpeed);
        }
    }
}