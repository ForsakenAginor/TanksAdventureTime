using UnityEngine;

namespace Enemies
{
    public class Mortar : EnemyWeapon
    {
        private readonly float AngleRadian;

        private MortarProjectile _current;

        public Mortar(
            MortarProjectile projectile,
            Transform viewPoint,
            IPlayerDetector target,
            float angle,
            AudioSource sound)
            : base(projectile, viewPoint, target, sound)
        {
            AngleRadian = angle * Mathf.Rad2Deg;
        }

        public override void OnShoot()
        {
            Pull<MortarProjectile>(ViewPoint, ViewPoint.position)
                .Move(AngleRadian, Target.Position - ViewPoint.position, ViewPoint.forward);
        }
    }
}