using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class OnDeathEffectInitializer : MonoBehaviour
    {
        [Header("Camera blend")]
        [SerializeField] private CinemachineVirtualCamera _followCamera;
        [SerializeField] private CinemachineVirtualCamera _deathCamera;
        [SerializeField] private CinemachineBrain _brain;

        [Header("Particle effects")]
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private ParticleSystem _fireEffect;
        [SerializeField] private Transform _spawnPoint;

        [Header("Cannon destruction")]
        [SerializeField] private Transform _cannon;
        [SerializeField] private float _effectSpeed;
        [SerializeField] private float _distance;
        [SerializeField] private float _height = 5f;

        [Header("AudioEffect")]
        [SerializeField] private AudioSource _audioSource;

        private ICancelableOnDeathEffect[] _onDeathEffects;
        private OnDeathCameraSwitcher _deathCameraSwitcher;
        private CannonDestruction _cannonDestructionEffect;
        private ExplosionSpawner _explosionSpawner;

        private void Awake()
        {
            _deathCameraSwitcher = new(_followCamera, _deathCamera, _brain.m_DefaultBlend.m_Time);
            _cannonDestructionEffect = new(_cannon, _effectSpeed, _distance, _height);
        }

        public void CreateEffect()
        {
            _explosionSpawner = gameObject.AddComponent<ExplosionSpawner>();
            _explosionSpawner.Init(_explosionEffect, _fireEffect, _spawnPoint.position);
            _deathCameraSwitcher.Switch();
            _cannonDestructionEffect.Detonate();
            _audioSource.Play();
            _onDeathEffects = new ICancelableOnDeathEffect[] { _deathCameraSwitcher, _cannonDestructionEffect, _explosionSpawner };
        }

        public void Init()
        {
            if (_onDeathEffects != null)
                _onDeathEffects.ToList().ForEach(o => o.ReturnToNormalState());
        }
    }
}