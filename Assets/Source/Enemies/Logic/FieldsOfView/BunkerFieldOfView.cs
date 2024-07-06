using UnityEngine;

namespace Enemies
{
    public class BunkerFieldOfView : StandardFieldOfView
    {
        private readonly float HalfAngle;

        public BunkerFieldOfView(
            ITarget target,
            Transform forwardPoint,
            float attackRadius,
            LayerMask walls,
            float halfAngle)
            : base(target, forwardPoint, attackRadius, walls)
        {
            HalfAngle = halfAngle;
        }

        public override bool CanView()
        {
            return IsPlayerInRadius() && IsPlayerInAngle();
        }

        private bool IsPlayerInAngle()
        {
            return Vector3.Angle(ViewPoint.forward, Target.Position - ViewPoint.position) <= HalfAngle;
        }
    }
}