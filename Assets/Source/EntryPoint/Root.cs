using Agava.WebUtility;
using Assets.Source.Player;
using Assets.Source.Player.HealthSystem;
using System;
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
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private GameObject _wonPanel;
        [SerializeField] private GameObject _mobileInputCanvas;

        [Header("Player")]
        [SerializeField] private PlayerBehaviour _player;
        [SerializeField] private PlayerDamageTaker _playerDamageTaker;
        [SerializeField] private GameObject _playerModel;
        [SerializeField] private PlayerInitializer _playerInitializer;
        [SerializeField] private OnDeathEffectInitializer _onDeathEffectInitializer;
        private Vector3 _spawnPoint;

        private void Start()
        {
            /* 
            uncomment that on publishing

            if(Device.IsMobile == false)
                _mobileInputCanvas.SetActive(false);
            */
            LevelConfiguration configuration = new(_smallMilitarySpots, _mediumMilitarySpots, _largeMilitarySpots);
            LevelGenerator levelGenerator = new(configuration, _buildingPresets, _buildingSpots, _spawner);
            _playerInitializer.Init(_playerDamageTaker, _player);
            _spawnPoint = _playerModel.transform.position;
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
            _player.enabled = true;
            _playerInitializer.Respawn();
        }


        private void OnPlayerDied()
        {
            _onDeathEffectInitializer.CreateEffect();
        }
    }
}