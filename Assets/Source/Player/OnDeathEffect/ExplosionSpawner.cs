using System;
using UnityEngine;

namespace Player
{
    public class ExplosionSpawner : MonoBehaviour, ICancelableOnDeathEffect
    {
        private ParticleSystem _explosionEffect;
        private ParticleSystem _firEffect;

        public void ReturnToNormalState()
        {
            if (_explosionEffect == null || _firEffect == null)
                throw new Exception("ExplosionSpawner class was not initialized yet");

            Destroy(_explosionEffect.gameObject);
            Destroy(_firEffect.gameObject);
        }

        public void Init(ParticleSystem explosionEffect, ParticleSystem fireEffect, Vector3 spanwPoint)
        {
            if (explosionEffect == null)
                throw new ArgumentNullException(nameof(explosionEffect));

            if (fireEffect == null)
                throw new ArgumentNullException(nameof(fireEffect));

            float effectScale = 2f;
            _explosionEffect = Instantiate(explosionEffect, spanwPoint, Quaternion.identity);
            _explosionEffect.transform.localScale = Vector3.one * effectScale;
            _firEffect = Instantiate(fireEffect, spanwPoint, Quaternion.identity);
            _firEffect.transform.localScale = Vector3.one * effectScale;
        }
    }
}