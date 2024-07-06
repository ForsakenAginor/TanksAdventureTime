using System;
using UnityEngine;

namespace Enemies
{
    public class FieldOfViewFactory : IEnemyFactory<IFieldOfView>
    {
        private readonly ITarget Target;
        private readonly Transform ViewPoint;
        private readonly float AttackRadius;
        private readonly LayerMask Walls;
        private readonly float Angle;
        private readonly float ProjectileRadius;
        private readonly float MortarTrajectoryOffset;

        public FieldOfViewFactory(
            ITarget target,
            Transform viewPoint,
            float attackRadius,
            LayerMask walls,
            float angle,
            float mortarTrajectoryOffset,
            float projectileRadius)
        {
            Target = target;
            ViewPoint = viewPoint;
            AttackRadius = attackRadius;
            Walls = walls;
            Angle = angle;
            ProjectileRadius = projectileRadius;
            MortarTrajectoryOffset = mortarTrajectoryOffset;
        }

        public IFieldOfView Create(EnemyTypes type)
        {
            return type switch
            {
                EnemyTypes.Standard => new StandardFieldOfView(Target, ViewPoint, AttackRadius, Walls),
                EnemyTypes.Mortar => new MortarFieldOfView(
                    Target,
                    ViewPoint,
                    AttackRadius,
                    Walls,
                    Angle,
                    ProjectileRadius,
                    MortarTrajectoryOffset),
                EnemyTypes.Bunker => new BunkerFieldOfView(
                    Target,
                    ViewPoint,
                    AttackRadius,
                    Walls,
                    Angle / (int)ValueConstants.Two),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}