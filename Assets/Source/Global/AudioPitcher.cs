using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioPitcher
{
    private readonly AudioSource _sound;
    private readonly float _minValue;
    private readonly float _maxValue;

    public AudioPitcher(AudioSource sound, float minValue, float maxValue)
    {
        _sound = sound ? sound : throw new NullReferenceException();
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public bool IsPlaying => _sound.isPlaying;

    public void Play()
    {
        if (_sound.clip == null)
            return;

        _sound.pitch = Random.Range(_minValue, _maxValue);
        _sound.Play();
    }
}