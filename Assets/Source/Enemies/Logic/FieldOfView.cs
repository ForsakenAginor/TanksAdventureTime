using UnityEngine;

namespace Enemies
{
    public class EnemyFieldOfView : IFieldOfView
    {
        private readonly ITarget Target;
        private readonly Transform Head;
        private readonly float AttackRadius;
        private readonly LayerMask Walls;
        private readonly RaycastHit[] Hits;

        public EnemyFieldOfView(ITarget target, Transform head, float attackRadius, LayerMask walls)
        {
            Target = target;
            Head = head;
            AttackRadius = attackRadius;
            Walls = walls;
            Hits = new RaycastHit[(int)ValueConstants.One];
        }

        public bool IsBlockingByWall()
        {
            return Physics.RaycastNonAlloc(
                Head.position,
                Target.Position - Head.position,
                Hits,
                AttackRadius,
                Walls) > 0;
        }

        public bool IsPlayerInRadius()
        {
            return Vector3.Distance(Head.position, Target.Position) <= AttackRadius;
        }
    }
}