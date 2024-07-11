using UnityEngine;

namespace Enemies
{
    public class StandardFieldOfView : EnemyFieldOfView
    {
        private readonly LayerMask _walls;
        private readonly RaycastHit[] _hits;

        public StandardFieldOfView(ITarget target, Transform viewPoint, float attackRadius, LayerMask walls)
            : base(target, viewPoint, attackRadius)
        {
            _walls = walls;
            _hits = new RaycastHit[(int)ValueConstants.One];
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
                _hits,
                AttackRadius,
                _walls) > 0;
        }
    }
}