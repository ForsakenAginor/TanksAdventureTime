using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class MortarFieldOfView : EnemyFieldOfView
    {
        private readonly LayerMask _walls;
        private readonly float _angleRadian;
        private readonly float _projectileRadius;
        private readonly float _heightOffset;

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
            _walls = walls;
            _angleRadian = angle * Mathf.Deg2Rad;
            _projectileRadius = projectileRadius;
            _heightOffset = heightOffset;
        }

        public override bool IsBlockingByWall()
        {
            Vector3 currentPosition = ViewPoint.position;
            Vector3 targetPosition = Target.Position;
            Vector3 direction = targetPosition - currentPosition;
            Vector3 forward = ViewPoint.forward.RotateAlongY(direction);
            List<Vector3> points = forward.CalculateTrajectory(
                currentPosition,
                targetPosition,
                direction,
                _angleRadian);

            Vector3 middleHeightPosition = points[points.Count / (int)ValueConstants.Two];
            middleHeightPosition.y += _heightOffset;
            Vector3 fromBottom = middleHeightPosition - currentPosition;
            Vector3 fromTop = targetPosition - middleHeightPosition;

            if (Physics.SphereCast(
                    currentPosition,
                    _projectileRadius,
                    fromBottom,
                    out _,
                    fromBottom.magnitude,
                    _walls) == true)
                return true;

            return Physics.SphereCast(
                middleHeightPosition,
                _projectileRadius,
                fromTop,
                out _,
                fromTop.magnitude,
                _walls);
        }
    }
}