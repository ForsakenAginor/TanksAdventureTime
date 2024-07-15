using UnityEngine;

namespace Characters
{
    public class MachineGun : CharacterWeapon<IDamageableTarget>
    {
        private readonly ParticleSystem _shootingEffect;

        public MachineGun(
            Transform viewPoint,
            IDamageableTarget target,
            AudioPitcher sound,
            ParticleSystem shootingEffect)
            : base(viewPoint, target, sound)
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