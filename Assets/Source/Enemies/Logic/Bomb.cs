using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Enemies
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private GameObject _colliderBody;
        [SerializeField] private string _ignoreLayer = "Ignore Raycast";

        private IExplosive _explosive;
        private Transform _transform;
        private bool _didExplode;
        private int _ignoreInteraction;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }

        private void OnCollisionEnter()
        {
            OnExploding();
        }

        public void Init(IExplosive explosive, Action<AudioSource> initCallback)
        {
            _explosive = explosive;
            _transform = transform;
            _ignoreInteraction = LayerMask.NameToLayer(_ignoreLayer);
            initCallback?.Invoke(_sound);
        }

        public void Explode()
        {
            if (_didExplode == true)
                return;

            _didExplode = true;
            gameObject.layer = _ignoreInteraction;
            _colliderBody.SetActive(false);
            _explosive.Explode(_transform.position, _explosionRadius);
            OnExploded().Forget();
        }

        public virtual void OnExploding()
        {
            Explode();
        }

        private async UniTaskVoid OnExploded()
        {
            _sound.Play();
            _particle.Play();

            while (_particle.isPlaying || _sound.isPlaying)
                await UniTask.NextFrame(destroyCancellationToken);

            Destroy(gameObject);
        }
    }
}