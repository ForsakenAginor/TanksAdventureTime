using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class MortarFieldOfView : EnemyFieldOfView
    {
        private readonly LayerMask Walls;
        private readonly float AngleRadian;
        private readonly float ProjectileRadius;
        private readonly float HeightOffset;

        public MortarFieldOfView(
            ITarget target,
            Transform viewPoint,
            float attackRadius,
            LayerMask walls,
            float angle,
            float projectileRadius,
            float heightOffset)
            : base(target, viewPoint, attackRadius)
        {
            Walls = walls;
            AngleRadian = angle * Mathf.Deg2Rad;
            ProjectileRadius = projectileRadius;
            HeightOffset = heightOffset;
        }

        public override bool IsBlockingByWall()
        {
            Vector3 currentPosition = ViewPoint.position;
            Vector3 targetPosition = Target.Position;
            Vector3 direction = targetPosition - currentPosition;
            Vector3 forward = ViewPoint.forward.RotateAlongY(direction);
            Vector3 gravity = Physics.gravity;
            Vector3 velocity = forward.CalculateVelocity(direction, AngleRadian, gravity.y);
            List<Vector3> points = new ();

            for (float i = 0; i < direction.magnitude; i += 0.2f)
            {
                Vector3 position = currentPosition + velocity * i + gravity * i * i / (float)ValueConstants.Two;

                if (position.y < targetPosition.y)
                    break;

                points.Add(position);
            }

            Vector3 middleHeightPosition = points[points.Count / (int)ValueConstants.Two];
            middleHeightPosition.y += HeightOffset;
            Vector3 fromBottom = middleHeightPosition - currentPosition;
            Vector3 fromTop = targetPosition - middleHeightPosition;

            if (Physics.SphereCast(
                    currentPosition,
                    ProjectileRadius,
                    fromBottom,
                    out _,
                    fromBottom.magnitude,
                    Walls) == true)
                return true;

            return Physics.SphereCast(
                middleHeightPosition,
                ProjectileRadius,
                fromTop,
                out _,
                fromTop.magnitude,
                Walls);
        }
    }
}