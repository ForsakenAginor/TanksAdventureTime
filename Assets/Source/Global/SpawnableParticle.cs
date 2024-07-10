using UnityEngine;

public class SpawnableParticle : SpawnableObject
{
    [SerializeField] private ParticleSystem _particle;

    public bool IsPlaying => _particle.isPlaying;

    public void Play()
    {
        _particle.Play();
    }

    public void Stop()
    {
        _particle.Stop();
    }
}