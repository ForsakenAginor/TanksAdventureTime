using Cinemachine;
using System.Linq;
using UnityEngine;

public class OnDeathEffectInitializer : MonoBehaviour
{
    [Header("Camera blend")]
    [SerializeField] private CinemachineVirtualCamera _followCamera;
    [SerializeField] private CinemachineVirtualCamera _deathCamera;
    [SerializeField] private CinemachineBrain _brain;

    [Header("Particle effects")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _fireEffect;
    [SerializeField] private Transform _spanwPoint;

    [Header("Cannon destruction")]
    [SerializeField] private Transform _cannon;
    [SerializeField] private float _effectSpeed;
    [SerializeField] private float _distance;
    [SerializeField] private float _heigth;

    private IOnDeathEffect[] _onDeathEffects;
    private OnDeathCameraSwitcher _deathCameraSwitcher;
    private CannonDestruction _cannonDestructionEffect;
    private ExplosionSpawner _explosionSpawner;

    private void Awake()
    {
        _deathCameraSwitcher = new (_followCamera, _deathCamera, _brain.m_DefaultBlend.m_Time);
        _cannonDestructionEffect = new(_cannon, _effectSpeed, _distance, _heigth);
    }

    public void CreateEffect()
    {
        _explosionSpawner = gameObject.AddComponent<ExplosionSpawner>();
        _explosionSpawner.Init(_explosionEffect, _fireEffect, _spanwPoint.position);
        _deathCameraSwitcher.Switch();
        _cannonDestructionEffect.Detonate();
        _onDeathEffects = new IOnDeathEffect[] { _deathCameraSwitcher, _cannonDestructionEffect, _explosionSpawner };
    }

    public void Init()
    {
        if(_onDeathEffects != null)
            _onDeathEffects.ToList().ForEach(o => o.ReturnToNormalState());
    }
}