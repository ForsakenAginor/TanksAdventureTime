using UnityEngine;

namespace Enemies
{
    public class StandardFieldOfView : EnemyFieldOfView
    {
        private readonly LayerMask Walls;
        private readonly RaycastHit[] Hits;

        public StandardFieldOfView(ITarget target, Transform viewPoint, float attackRadius, LayerMask walls)
            : base(target, viewPoint, attackRadius)
        {
            Walls = walls;
            Hits = new RaycastHit[(int)ValueConstants.One];
        }

        public override bool IsBlockingByWall()
        {
            return IsRaycastHit();
        }

        private bool IsRaycastHit()
        {
            return Physics.RaycastNonAlloc(
                ViewPoint.position,
                Target.Position - ViewPoint.position,
                Hits,
                AttackRadius,
                Walls) > 0;
        }
    }
}