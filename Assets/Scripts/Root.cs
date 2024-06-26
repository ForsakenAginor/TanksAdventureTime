using System;
using UnityEngine;

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

    private void Start()
    {
        LevelConfiguration configuration = new (_smallMilitarySpots, _mediumMilitarySpots, _largeMilitarySpots);
        LevelGenerator levelGenerator = new (configuration, _buildingPresets, _buildingSpots, _spawner);
    }

    public void Respawn()
    {
        throw new NotImplementedException();
    }
}
