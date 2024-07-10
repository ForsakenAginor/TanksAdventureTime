using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioPitcher
{
    private readonly AudioSource Sound;
    private readonly float MinValue;
    private readonly float MaxValue;

    public AudioPitcher(AudioSource sound, float minValue, float maxValue)
    {
        Sound = sound ? sound : throw new NullReferenceException();
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public bool IsPlaying => Sound.isPlaying;

    public void Play()
    {
        if (Sound.clip == null)
            return;

        Sound.pitch = Random.Range(MinValue, MaxValue);
        Sound.Play();
    }
}