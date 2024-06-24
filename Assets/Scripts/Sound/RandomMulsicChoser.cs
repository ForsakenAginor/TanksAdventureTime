using System;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomMulsicChoser : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _clips;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (_clips.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(_clips));

            int clipNumber = UnityEngine.Random.Range(0, _clips.Length);
            _audioSource.clip = _clips[clipNumber];
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}