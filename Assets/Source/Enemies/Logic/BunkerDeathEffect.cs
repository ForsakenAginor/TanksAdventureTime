using UnityEngine;

namespace Enemies
{
    public class BunkerDeathEffect : IDeath
    {
        private readonly ParticleSystem _particle;
        private readonly AudioSource _sound;

        public BunkerDeathEffect(ParticleSystem particle, AudioSource sound)
        {
            _particle = particle;
            _sound = sound;
        }

        public void Die()
        {
            _particle.Play();
            _sound.Play();
        }
    }
}