using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Mortar : EnemyWeapon<IDamageableTarget>
    {
        private readonly IProjectileFactory Factory;

        public Mortar(
            Transform viewPoint,
            IDamageableTarget target,
            AudioPitcher sound,
            IProjectileFactory factory)
            : base(viewPoint, target, sound)
        {
            Factory = factory;
        }

        public override void OnShoot()
        {
            Factory.Create(ViewPoint.position, Target.Position, Target.Position - ViewPoint.position, ViewPoint.forward);
        }
    }
}