using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private int _smallMilitarySpots;
    [SerializeField] private int _mediumMilitarySpots;
    [SerializeField] private int _largeMilitarySpots;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private BuildingPresetCollection _buildingPresets;
    [SerializeField] private BuildingSpotsCollection _buildingSpots;

    private void Start()
    {
        LevelConfiguration configuration = new (_smallMilitarySpots, _mediumMilitarySpots, _largeMilitarySpots);
        LevelGenerator levelGenerator = new (configuration, _buildingPresets, _buildingSpots, _spawner);
    }
}
