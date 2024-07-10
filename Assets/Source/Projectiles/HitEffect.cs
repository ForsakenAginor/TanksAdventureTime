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

        public void Init(Vector3 position)
        {
            Transform.rotation = Quaternion.LookRotation(position - Transform.position);
            Init();
        }

        public void Init()
        {
            _sound ??= new AudioPitcher(GetComponent<AudioSource>(), _minPitch, _maxPitch);

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