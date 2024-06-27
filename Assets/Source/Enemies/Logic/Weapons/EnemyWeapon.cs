using UnityEngine;

namespace Enemies
{
    public abstract class EnemyWeapon : ObjectPool, IWeapon
    {
        private const float MaxPitch = 1.3f;
        private const float MinPitch = 1f;

        private readonly AudioSource Sound;

        public EnemyWeapon(SpawnableObject spawnable, Transform viewPoint, IPlayerTarget target, AudioSource sound)
            : base(spawnable)
        {
            ViewPoint = viewPoint;
            Target = target;
            Sound = sound;
        }

        public Transform ViewPoint { get; }

        public IPlayerTarget Target { get; }

        public void Shoot()
        {
            PlaySound();
            OnShoot();
        }

        public virtual void OnShoot()
        {
        }

        private void PlaySound()
        {
            Sound.pitch = Random.Range(MinPitch, MaxPitch);
            Sound.Play();
        }
    }
}