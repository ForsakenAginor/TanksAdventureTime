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
using SavingData;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Button _startButton;

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
        private CurrencyCalculator _currencyCalculator;
        private int _currentLevel;

        [Header("Other")]
        [SerializeField] private SaveService _saveService;
        [SerializeField] private Goods _goods;
        [SerializeField] private Silencer _silencer;
        private Dictionary<GoodNames, object> _savedData;

        public Action PlayerDied;
        public Action PlayerRespawned;

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
            DifficultySystem difficultySystem = new(_currentLevel);

            if (difficultySystem.CurrentConfiguration.Bunkers > 0 && _saveService.HadHelper == false)
            {
                _uIManager.ShowHelperAttentionPanel();
                _saveService.SavePlayerHelperData((int)PlayerHelperTypes.MachineGun);
            }

            LevelGenerator levelGenerator = new(difficultySystem.CurrentConfiguration,
                                                _buildingPresets,
                                                _buildingSpots,
                                                _spawner,
                                                _playerDamageTaker,
                                                OnAudioCreated,
                                                OnEnemySpawned);

            _savedData = _saveService.GetPurchasesData().GetContent(_goods);
            _spawnPoint = _playerDamageTaker.transform.position;
            _enemiesManager = new(_enemies);

            if (_saveService.HadHelper)
                _playerHelper.Init(_enemies, (PlayerHelperTypes)_saveService.Helper, OnAudioCreated, HelperInitCallback);
            else
                _playerHelper.Init();

            _winCondition.Init(_enemiesManager.AlivedEnemies);
            _uIManager.Init(_enemiesManager.AlivedEnemies, _playerDamageTaker.transform, _currentLevel);
            Wallet wallet = new(_saveService);
            _currencyCalculator = new(_bounty, wallet);
            InterstitialAdvertiseShower advertiseShower = new(_silencer);

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
            PlayerRespawned?.Invoke();
        }

        private void OnPlayerWon()
        {
            _playerBehaviour.Stop();
            LeaderboardScoreSaver leaderboardScoreSaver = new();

#if UNITY_WEBGL && !UNITY_EDITOR
            leaderboardScoreSaver.SaveScore(_currentLevel);
#endif
            _saveService.SaveLevelData(++_currentLevel);
            _uIManager.ShowWiningPanel();
            _victoryEffect.PlayEffect(_enemies.Count, _currencyCalculator.CalculateTotalBounty(_enemies.Count));
        }


        private void OnPlayerDied()
        {
            _playerBehaviour.Stop();
            PlayerDied?.Invoke();
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

        private void InitializePlayer()
        {
            _playerInitializer.Init(_savedData, _playerDamageTaker, _playerBehaviour, OnAudioCreated);
        }
    }
}