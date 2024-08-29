using System;
using UnityEngine;

namespace Player
{
    public class ExplosionSpawner : MonoBehaviour, ICancelableOnDeathEffect
    {
        private const string ExceptionMessage = "ExplosionSpawner class was not initialized yet";

        private ParticleSystem _explosionEffect;
        private ParticleSystem _firEffect;

        public void ReturnToNormalState()
        {
            if (_explosionEffect == null || _firEffect == null)
                throw new Exception(ExceptionMessage);

            Destroy(_explosionEffect.gameObject);
            Destroy(_firEffect.gameObject);
        }

        public void Init(ParticleSystem explosionEffect, ParticleSystem fireEffect, Vector3 spawnPoint)
        {
            if (explosionEffect == null)
                throw new ArgumentNullException(nameof(explosionEffect));

            if (fireEffect == null)
                throw new ArgumentNullException(nameof(fireEffect));

            float effectScale = 2f;
            _explosionEffect = Instantiate(explosionEffect, spawnPoint, Quaternion.identity);
            _explosionEffect.transform.localScale = Vector3.one * effectScale;
            _firEffect = Instantiate(fireEffect, spawnPoint, Quaternion.identity);
            _firEffect.transform.localScale = Vector3.one * effectScale;
        }
    }
}