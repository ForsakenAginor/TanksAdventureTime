using System;
using System.Collections.Generic;
using Advertise;
using Difficulty;
using Enemies;
using LevelGeneration;
using Player;
using PlayerHelpers;
using SavingProgress;
using Shops;
using Sound;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace EntryPoint
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
        [SerializeField] private Button _startButton;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private PlayerDamageTaker _playerDamageTaker;
        [SerializeField] private PlayerInitializer _playerInitializer;
        [SerializeField] private OnDeathEffectInitializer _onDeathEffectInitializer;
        [SerializeField] private PlayerHelperSetup _playerHelper;
        private Vector3 _spawnPoint;

        [Header("Enemies")]
        private readonly List<IDamageableTarget> _enemies = new ();
        private EnemiesManager _enemiesManager;

        [Header("Audio")]
        [SerializeField] private SoundInitializer _soundInitializer;

        [Header("GameProgress")]
        [SerializeField] private WinCondition _winCondition;
        [SerializeField] private int _bounty;
        private CurrencyCalculator _currencyCalculator;
        private int _currentLevel;

        [Header("Other")]
        [SerializeField] private SaveService _saveService;
        [SerializeField] private Goods _goods;
        [SerializeField] private Silencer _silencer;
        private Dictionary<GoodNames, object> _savedData;

        private Action _playerDied;
        private Action _playerRespawned;

        private void OnEnable()
        {
            _saveService.Loaded += Init;
            _playerDamageTaker.PlayerDied += OnPlayerDied;
            _winCondition.PlayerWon += OnPlayerWon;
            _startButton.onClick.AddListener(InitializePlayer);
        }

        private void OnDisable()
        {
            _saveService.Loaded -= Init;
            _playerDamageTaker.PlayerDied -= OnPlayerDied;
            _winCondition.PlayerWon -= OnPlayerWon;
            _startButton.onClick.RemoveListener(InitializePlayer);
        }

        private void Init()
        {
            _soundInitializer.Init();
            _currentLevel = _saveService.Level;
            DifficultySystem difficultySystem = new (_currentLevel);

            if (difficultySystem.CurrentConfiguration.Bunkers > 0 && _saveService.HadHelper == false)
            {
                _uIManager.ShowHelperAttentionPanel();
                _saveService.SavePlayerHelper((int)PlayerHelperTypes.MachineGun);
            }

            LevelGenerator levelGenerator = new (difficultySystem.CurrentConfiguration,
                                                _buildingPresets,
                                                _buildingSpots,
                                                _spawner,
                                                _playerDamageTaker,
                                                OnAudioCreated,
                                                OnEnemySpawned);

            _savedData = _saveService.GetPurchases().GetContent(_goods);
            _spawnPoint = _playerDamageTaker.transform.position;
            _enemiesManager = new (_enemies);

            if (_saveService.HadHelper)
                _playerHelper.Init(_enemies, (PlayerHelperTypes)_saveService.Helper, OnAudioCreated, HelperInitCallback);
            else
                _playerHelper.Init();

            _winCondition.Init(_enemiesManager.AliveEnemies);
            _uIManager.Init(_enemiesManager.AliveEnemies, _playerDamageTaker.transform, _currentLevel);
            Wallet wallet = new (_saveService);
            _currencyCalculator = new (_bounty, wallet);
            InterstitialAdvertiseShower advertiseShower = new (_silencer);

#if UNITY_WEBGL && !UNITY_EDITOR
            StickyAd.Show();
            advertiseShower.ShowAdvertise();
#endif

            _startButton.interactable = true;
            Time.timeScale = 0f;
        }

        public void Respawn()
        {
            _playerDamageTaker.transform.SetPositionAndRotation(_spawnPoint, Quaternion.identity);
            _onDeathEffectInitializer.Init();
            _playerBehaviour.Continue();
            _playerRespawned?.Invoke();
        }

        private void OnPlayerWon()
        {
            _playerBehaviour.Stop();
            LeaderboardScoreSaver leaderboardScoreSaver = new ();

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
            _playerDied?.Invoke();
            _onDeathEffectInitializer.CreateEffect();
            _uIManager.ShowLosingPanel();
        }

        private void HelperInitCallback((Action onEnable, Action onDisable) tuple)
        {
            _playerDied = tuple.onDisable;
            _playerRespawned = tuple.onEnable;
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

        private void InitializePlayer()
        {
            _playerInitializer.Init(_savedData, _playerDamageTaker, _playerBehaviour, OnAudioCreated);
        }
    }
}