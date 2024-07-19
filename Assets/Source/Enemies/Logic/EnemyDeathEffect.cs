using System.Threading;
using Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    public class EnemyDeathEffect : MonoBehaviour
    {
        private ParticleSystem _particle;
        private AudioSource _sound;
        private CharacterAnimation _animation;
        private float _disappearDuration;
        private int _layer;
        private CancellationToken _token;

        private bool _isDying;

        public void Init(
            ParticleSystem particle,
            AudioSource sound,
            CharacterAnimation animation,
            float disappearDuration,
            int layer,
            CancellationToken token)
        {
            _particle = particle;
            _sound = sound;
            _animation = animation;
            _disappearDuration = disappearDuration;
            _layer = layer;
            _token = token;
        }

        public void Die()
        {
            if (_isDying == true)
                return;

            gameObject.layer = _layer;
            _isDying = true;
            _particle.Play();
            _sound.Play();
            _animation.Play(CharacterAnimations.Death);
            Disappear().Forget();
        }

        private async UniTaskVoid Disappear()
        {
            await UniTask.WaitUntil(
                () => _particle.isPlaying == false && _sound.isPlaying == false && _animation.IsPlaying() == false,
                cancellationToken: _token);

            gameObject.SetActive(false);
        }
    }
}