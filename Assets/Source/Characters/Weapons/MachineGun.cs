using UnityEngine;

namespace Characters
{
    public class MachineGun : CharacterWeapon<IDamageableTarget>
    {
        private readonly ParticleSystem _shootingEffect;

        public MachineGun(
            Transform viewPoint,
            IDamageableTarget target,
            ParticleSystem shootingEffect)
            : base(viewPoint, target)
        {
            _shootingEffect = shootingEffect;
        }

        public override void OnShoot()
        {
            _shootingEffect.Play();
            Target.TakeHit(HitTypes.Bullet);
        }
    }
}