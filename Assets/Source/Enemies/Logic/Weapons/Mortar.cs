using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Mortar : EnemyWeapon<IDamageableTarget>
    {
        private readonly IProjectileFactory _factory;

        public Mortar(
            Transform viewPoint,
            IDamageableTarget target,
            AudioPitcher sound,
            IProjectileFactory factory)
            : base(viewPoint, target, sound)
        {
            _factory = factory;
        }

        public override void OnShoot()
        {
            _factory.Create(ViewPoint.position, Target.Position, Target.Position - ViewPoint.position, ViewPoint.forward);
        }
    }
}