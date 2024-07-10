using Assets.Source.Player;
using Assets.Source.Player.HealthSystem;
using Assets.Source.Player.OnDeathEffect;
using Assets.Source.Sound.AudioMixer;
using Enemies;
using UnityEngine;

namespace Assets.Source.EntryPoint
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private int _smallMilitarySpots;
        [SerializeField] private int _mediumMilitarySpots;
        [SerializeField] private int _largeMilitarySpots;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private BuildingPresetCollection _buildingPresets;
        [SerializeField] private BuildingSpotsCollection _buildingSpots;

        [Header("UI objects")]
        [SerializeField] private UIManager _uIManager;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private PlayerDamageTaker _playerDamageTaker;
        [SerializeField] private GameObject _playerModel;
        [SerializeField] private PlayerInitializer _playerInitializer;
        [SerializeField] private OnDeathEffectInitializer _onDeathEffectInitializer;
        private Vector3 _spawnPoint;

        [Header("Audio")]
        [SerializeField] private SoundInitializer _soundInitializer;

        [Header("Test")]
        [SerializeField] private EnemyTest _enemyTest;

        private void Start()
        {
            LevelConfiguration configuration = new (_smallMilitarySpots, _mediumMilitarySpots, _largeMilitarySpots);
            LevelGenerator levelGenerator = new (configuration, _buildingPresets, _buildingSpots, _spawner);
            _playerInitializer.Init(_playerDamageTaker, _playerBehaviour);
            _spawnPoint = _playerModel.transform.position;

            _soundInitializer.Init();
            _enemyTest.Init(_soundInitializer);
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
            _playerModel.transform.position = _spawnPoint;
            _playerModel.transform.rotation = Quaternion.identity;
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
    }
}