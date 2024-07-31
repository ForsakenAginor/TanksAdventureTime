using Agava.YandexGames;
using Assets.Source.Advertise;
using Assets.Source.Difficulty;
using Assets.Source.Enemies;
using Assets.Source.Global;
using Assets.Source.LevelGeneration;
using Assets.Source.Player;
using Assets.Source.Player.HealthSystem;
using Assets.Source.Player.OnDeathEffect;
using Assets.Source.Sound.AudioMixer;
using Assets.Source.UI;
using Assets.Source.UI.Menu.Leaderboard;
using PlayerHelpers;
using System;
using System.Collections.Generic;
using Shops;
using UnityEngine;

namespace Assets.Source.EntryPoint
{
    public class Root : MonoBehaviour
    {
        [Header("Level generation")]
        [SerializeField] private Spawner _spawner;
        [SerializeField] private PointPresetCollection _buildingPresets;
        [SerializeField] private PointsSpotsCollection _buildingSpots;

        [Header("UI objects")]
        [SerializeField] private UIManager _uIManager;
        [SerializeField] private VictoryEffect _victoryEffect;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private PlayerDamageTaker _playerDamageTaker;
        [SerializeField] private PlayerInitializer _playerInitializer;
        [SerializeField] private OnDeathEffectInitializer _onDeathEffectInitializer;
        [SerializeField] private PlayerHelperSetup _playerHelper;
        private Vector3 _spawnPoint;

        [Header("Enemies")]
        private readonly List<IDamageableTarget> _enemies = new();
        private EnemiesManager _enemiesManager;

        [Header("Audio")]
        [SerializeField] private SoundInitializer _soundInitializer;

        [Header("GameProgress")]
        [SerializeField] private WinCondition _winCondition;
        [SerializeField] private int _bounty;
        [SerializeField] private SaveService _saveService;
        private CurrencyCalculator _currencyCalculator;
        private int _currentLevel;

        [Header("Other")]
        [SerializeField] private Silencer _silencer;

        public Action PlayerDied;
        public Action PlayerRespawned;

        private void Start()
        {
            _soundInitializer.Init();
            DifficultySystem difficultySystem = new(_currentLevel);

            LevelGenerator levelGenerator = new(difficultySystem.CurrentConfiguration,
                                                _buildingPresets,
                                                _buildingSpots,
                                                _spawner,
                                                _playerDamageTaker,
                                                OnAudioCreated,
                                                OnEnemySpawned);
            _playerInitializer.Init(_playerDamageTaker, _playerBehaviour, OnAudioCreated);
            _spawnPoint = _playerDamageTaker.transform.position;

            _enemiesManager = new(_enemies);
            _playerHelper.Init(_enemies, PlayerHelperTypes.MachineGun, OnAudioCreated, HelperInitCallback);
            _winCondition.Init(_enemiesManager.AlivedEnemies);

            CurrencyData currencyData = new();
            Wallet wallet = new(currencyData);
            _currencyCalculator = new(_bounty, wallet);

            InterstitialAdvertiseShower advertiseShower = new(_silencer);

#if UNITY_WEBGL && !UNITY_EDITOR
            StickyAd.Show();
            advertiseShower.ShowAdvertise();
#endif
            Time.timeScale = 0f;
        }

        private void OnEnable()
        {
            _saveService.Loaded += SetLevel;
            _playerDamageTaker.PlayerDied += OnPlayerDied;
            _winCondition.PlayerWon += OnPlayerWon;
        }

        private void OnDisable()
        {
            _saveService.Loaded -= SetLevel;
            _playerDamageTaker.PlayerDied -= OnPlayerDied;
            _winCondition.PlayerWon -= OnPlayerWon;
        }


        public void Respawn()
        {
            _playerDamageTaker.transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
            _onDeathEffectInitializer.Init();
            _playerDamageTaker.Respawn();
            _playerBehaviour.Continue();
            PlayerRespawned.Invoke();
        }

        private void SetLevel()
        {
            _currentLevel = _saveService.Level;
            _uIManager.Init(_enemiesManager.AlivedEnemies, _playerDamageTaker.transform, _currentLevel);
        }

        private void OnPlayerWon()
        {
            _playerBehaviour.Stop();
            LeaderboardScoreSaver leaderboardScoreSaver = new();

#if UNITY_WEBGL && !UNITY_EDITOR
            leaderboardScoreSaver.SaveScore(_currentLevel);
#endif
            _saveService.SaveLevel(++_currentLevel);
            _uIManager.ShowWiningPanel();
            _victoryEffect.PlayEffect(_enemies.Count, _currencyCalculator.CalculateTotalBounty(_enemies.Count));
        }


        private void OnPlayerDied()
        {
            _playerBehaviour.Stop();
            PlayerDied.Invoke();
            _onDeathEffectInitializer.CreateEffect();
            _uIManager.ShowLosingPanel();
        }

        private void HelperInitCallback((Action onEnable, Action onDisable) tuple)
        {
            PlayerDied = tuple.onDisable;
            PlayerRespawned = tuple.onEnable;
        }

        private void OnAudioCreated(AudioSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _soundInitializer.AddEffectSource(source);
        }

        private void OnEnemySpawned(IDamageableTarget target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            _enemies.Add(target);
        }
    }
}