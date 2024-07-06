using UnityEngine;

namespace Enemies
{
    public class EnemyFieldOfView : IFieldOfView
    {
        public EnemyFieldOfView(ITarget target, Transform viewPoint, float attackRadius)
        {
            Target = target;
            ViewPoint = viewPoint;
            AttackRadius = attackRadius;
        }

        public ITarget Target { get; }
        public Transform ViewPoint { get; }
        public float AttackRadius { get; }

        public virtual bool IsBlockingByWall()
        {
            return false;
        }

        public virtual bool CanView()
        {
            return IsPlayerInRadius();
        }

        public bool IsPlayerInRadius()
        {
            return Vector3.Distance(ViewPoint.position, Target.Position) <= AttackRadius;
        }
    }
}