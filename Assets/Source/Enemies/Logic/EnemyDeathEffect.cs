using System.Threading;
using Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    public class EnemyDeathEffect : IDeath
    {
        private readonly ParticleSystem _particle;
        private readonly AudioSource _sound;
        private readonly CharacterAnimation _animation;
        private readonly GameObject _gameObject;
        private readonly int _layer;
        private readonly CancellationToken _token;

        private bool _isDying;

        public EnemyDeathEffect(
            ParticleSystem particle,
            AudioSource sound,
            CharacterAnimation animation,
            GameObject gameObject,
            int layer,
            CancellationToken token)
        {
            _particle = particle;
            _sound = sound;
            _animation = animation;
            _gameObject = gameObject;
            _layer = layer;
            _token = token;
        }

        public void Die()
        {
            if (_isDying == true)
                return;

            _gameObject.layer = _layer;
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

            _gameObject.SetActive(false);
        }
    }
}