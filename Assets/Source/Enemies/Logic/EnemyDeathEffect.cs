﻿using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyDeathEffect : MonoBehaviour
    {
        private Transform _transform;
        private ParticleSystem _particle;
        private AudioSource _sound;
        private EnemyAnimation _animation;
        private float _disappearDuration;
        private int _layer;
        private CancellationToken _token;

        public void Init(
            Transform transform,
            ParticleSystem particle,
            AudioSource sound,
            EnemyAnimation animation,
            float disappearDuration,
            int layer,
            CancellationToken token)
        {
            _transform = transform;
            _particle = particle;
            _sound = sound;
            _animation = animation;
            _disappearDuration = disappearDuration;
            _layer = layer;
            _token = token;
        }

        public void Die()
        {
            gameObject.layer = _layer;
            _animation.Play(EnemyAnimations.Death);
            _particle.Play();
            _sound.Play();
            Disappear().Forget();
        }

        private async UniTaskVoid Disappear()
        {
            while (_particle.isPlaying == true || _sound.isPlaying == true || _animation.IsPlaying() == true)
                await UniTask.NextFrame(_token);

            _transform.DOScale(Vector3.zero, _disappearDuration).OnComplete(() => Destroy(gameObject));
        }
    }
}