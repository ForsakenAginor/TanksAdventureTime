using UnityEngine;

namespace Enemies
{
    public class BunkerFieldOfView : StandardFieldOfView
    {
        private readonly float _halfAngle;

        public BunkerFieldOfView(
            ITarget target,
            Transform forwardPoint,
            float attackRadius,
            LayerMask walls,
            float halfAngle)
            : base(target, forwardPoint, attackRadius, walls)
        {
            _halfAngle = halfAngle;
        }

        public override bool CanView()
        {
            return IsPlayerInRadius() && IsPlayerInAngle();
        }

        private bool IsPlayerInAngle()
        {
            return Vector3.Angle(ViewPoint.forward, Target.Position - ViewPoint.position) <= _halfAngle;
        }
    }
}