using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(AudioSource))]
    public class HitEffect : SpawnableParticle
    {
        [Range(0f, 2f)]
        [SerializeField] private float _minPitch = 1f;
        [Range(0f, 2f)]
        [SerializeField] private float _maxPitch = 1.3f;

        private AudioPitcher _sound;

        public void Init(Vector3 position, Action<AudioSource> audioCreationCallback)
        {
            Transform.rotation = Quaternion.LookRotation(position - Transform.position);
            Init(audioCreationCallback);
        }

        public void Init(Action<AudioSource> audioCreationCallback)
        {
            if (_sound == null)
            {
                AudioSource source = GetComponent<AudioSource>();
                _sound = new AudioPitcher(GetComponent<AudioSource>(), _minPitch, _maxPitch);
                audioCreationCallback?.Invoke(source);
            }

            StartPlaying().Forget();
        }

        private async UniTaskVoid StartPlaying()
        {
            Play();
            _sound.Play();

            while (IsPlaying == true || _sound.IsPlaying == true)
                await UniTask.NextFrame(destroyCancellationToken);

            Push();
        }
    }
}