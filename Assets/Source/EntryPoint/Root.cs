using Assets.Source.Enemies;
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
        [SerializeField] private int _obstaclesSpots;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private PointPresetCollection _buildingPresets;
        [SerializeField] private PointsSpotsCollection _buildingSpots;

        [Header("UI objects")]
        [SerializeField] private UIManager _uIManager;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private PlayerDamageTaker _playerDamageTaker;
        [SerializeField] private PlayerInitializer _playerInitializer;
        [SerializeField] private OnDeathEffectInitializer _onDeathEffectInitializer;
        private Vector3 _spawnPoint;

        [Header("Enemies")]
        private readonly List<IDamageableTarget> _enemies = new ();
        private EnemiesManager _enemiesManager;

        [Header("Audio")]
        [SerializeField] private SoundInitializer _soundInitializer;

        [Header("GameProgress")]
        [SerializeField] private WinCondition _winCondition;

        private void Start()
        {
            _soundInitializer.Init();
            LevelConfiguration configuration = new (_smallMilitarySpots, _mediumMilitarySpots, _largeMilitarySpots, _obstaclesSpots);
            LevelGenerator levelGenerator = new (configuration, _buildingPresets, _buildingSpots, _spawner, _playerDamageTaker, OnAudioCreated, OnEnemySpawned);
            _playerInitializer.Init(_playerDamageTaker, _playerBehaviour, OnAudioCreated);
            _spawnPoint = _playerDamageTaker.transform.position;

            _enemiesManager = new (_enemies);
            _winCondition.Init(_enemiesManager.AlivedEnemies);

            _uIManager.Init(_enemiesManager.AlivedEnemies, _playerDamageTaker.transform);
        }

        private void OnEnable()
        {
            _playerDamageTaker.PlayerDied += OnPlayerDied;
            _winCondition.PlayerWon += OnPlayerWon;
        }

        private void OnDisable()
        {
            _playerDamageTaker.PlayerDied -= OnPlayerDied;            
            _winCondition.PlayerWon -= OnPlayerWon;
        }

        public void Respawn()
        {
            _playerDamageTaker.transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
            _onDeathEffectInitializer.Init();
            _playerDamageTaker.Respawn();
            _playerBehaviour.Continue();
        }

        private void OnPlayerWon()
        {
            //TODO save results
            _playerBehaviour.Stop();
            _uIManager.ShowWiningPanel();
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

        private void OnEnemySpawned(IDamageableTarget target)
        {
            if(target == null)
                throw new ArgumentNullException(nameof(target));

            _enemies.Add(target);
        }    
    }
}