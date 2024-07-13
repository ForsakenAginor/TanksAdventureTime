using Assets.Source.LevelGeneration;
using Assets.Source.Player;
using Assets.Source.Player.HealthSystem;
using Assets.Source.Player.OnDeathEffect;
using Assets.Source.Sound.AudioMixer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.EntryPoint
{
    public class Root : MonoBehaviour
    {
        [Header("Level generation")]
        [SerializeField] private int _smallMilitarySpots;
        [SerializeField] private int _mediumMilitarySpots;
        [SerializeField] private int _largeMilitarySpots;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private PointPresetCollection _buildingPresets;
        [SerializeField] private PointsSpotsCollection _buildingSpots;

        [Header("UI objects")]
        [SerializeField] private UIManager _uIManager;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private PlayerDamageTaker _playerDamageTaker;
        [SerializeField] private PlayerAsTarget _playerAsTarget;
        [SerializeField] private PlayerInitializer _playerInitializer;
        [SerializeField] private OnDeathEffectInitializer _onDeathEffectInitializer;
        private Vector3 _spawnPoint;

        [Header("Enemies")]
        private IEnumerable<ITarget> _enemies;

        [Header("Audio")]
        [SerializeField] private SoundInitializer _soundInitializer;

        private void Start()
        {
            _soundInitializer.Init();
            LevelConfiguration configuration = new (_smallMilitarySpots, _mediumMilitarySpots, _largeMilitarySpots);
            LevelGenerator levelGenerator = new (configuration, _buildingPresets, _buildingSpots, _spawner, _playerAsTarget, OnAudioCreated, OnEnemySpawned);
            _playerInitializer.Init(_playerDamageTaker, _playerBehaviour, _soundInitializer);
            _spawnPoint = _playerAsTarget.transform.position;

        }

        private void OnEnable()
        {
            _playerDamageTaker.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerDamageTaker.PlayerDied -= OnPlayerDied;            
        }

        public void Respawn()
        {
            _playerAsTarget.transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
            _onDeathEffectInitializer.Init();
            _playerDamageTaker.Respawn();
            _playerBehaviour.Continue();
        }


        private void OnPlayerDied()
        {
            _playerBehaviour.Stop();
            _onDeathEffectInitializer.CreateEffect();
            _uIManager.ShowLosingPanel();
        }

        private void OnAudioCreated(AudioSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _soundInitializer.AddEffectSource(source);
        }

        private void OnEnemySpawned(IEnumerable<ITarget> targets)
        {
            _enemies = targets != null ? targets : throw new ArgumentNullException(nameof(targets));
        }    
    }
}