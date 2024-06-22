using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator
{
    private readonly BuildingPresetCollection _presets;
    private readonly List<Vector3> _smallSpots;
    private readonly List<Vector3> _mediumSpots;
    private readonly List<Vector3> _largeSpots;
    private readonly LevelConfiguration _configuration;
    private readonly Spawner _spawner;

    public LevelGenerator(LevelConfiguration configuration, BuildingPresetCollection presets, BuildingSpotsCollection spots, Spawner spawner)
    {
        _configuration = configuration != null ? configuration : throw new ArgumentNullException(nameof(configuration));
        _presets = presets != null ? presets : throw new ArgumentNullException(nameof(presets));
        _spawner = spawner != null ? spawner : throw new ArgumentNullException(nameof(spawner));

        if (spots == null)
            throw new ArgumentNullException(nameof(spots));

        _smallSpots = spots.SmallBuildingSpots.ToList();
        _mediumSpots = spots.MediumBuildingSpots.ToList();
        _largeSpots = spots.LargeBuildingSpots.ToList();

        Generate();
    }

    private void Generate()
    {
        GameObject[] smallMilitaryPresets = ChosePresets(BuildingPresetType.Military, BuildingPresetSize.Small);
        GameObject[] mediumMilitaryPresets = ChosePresets(BuildingPresetType.Military, BuildingPresetSize.Medium);
        GameObject[] largeMilitaryPresets = ChosePresets(BuildingPresetType.Military, BuildingPresetSize.Large);
        GameObject[] smallCivilianPresets = ChosePresets(BuildingPresetType.Civilian, BuildingPresetSize.Small);
        GameObject[] mediumCivilianPresets = ChosePresets(BuildingPresetType.Civilian, BuildingPresetSize.Medium);
        GameObject[] largeCivilianPresets = ChosePresets(BuildingPresetType.Civilian, BuildingPresetSize.Large);

        if (_smallSpots.Count() < _configuration.MilitarySmallBuildings)
            throw new Exception("wrong level configuration: not enought small spots)");

        int amount = _configuration.MilitarySmallBuildings;
        SpawnRandomBuildings(amount, smallMilitaryPresets, _smallSpots);
        amount = _smallSpots.Count();
        SpawnRandomBuildings(amount, smallCivilianPresets, _smallSpots);

        if (_mediumSpots.Count() < _configuration.MilitaryMediumBuildings)
            throw new Exception("wrong level configuration: not enought medium spots)");

        amount = _configuration.MilitaryMediumBuildings;
        SpawnRandomBuildings(amount, mediumMilitaryPresets, _mediumSpots);
        amount = _mediumSpots.Count();
        SpawnRandomBuildings(amount, mediumCivilianPresets, _mediumSpots);

        if (_largeSpots.Count() < _configuration.MilitaryLargeBuildings)
            throw new Exception("wrong level configuration: not enought large spots)");

        amount = _configuration.MilitaryLargeBuildings;
        SpawnRandomBuildings(amount, largeMilitaryPresets, _largeSpots);
        amount = _largeSpots.Count();
        SpawnRandomBuildings(amount, largeCivilianPresets, _largeSpots);
    }

    private void SpawnRandomBuildings(int amount, GameObject[] buildings, List<Vector3> spots)
    {
        while (amount > 0)
        {
            int presetIndex = UnityEngine.Random.Range(0, buildings.Length);
            Vector3 spot = spots[UnityEngine.Random.Range(0, spots.Count)];
            _spawner.Spawn(spot, buildings[presetIndex]);
            spots.Remove(spot);
            amount--;
        }
    }

    private GameObject[] ChosePresets(BuildingPresetType type, BuildingPresetSize size)
    {
        return _presets.Presets.Where(o => o.Size == size && o.Type == type).Select(o => o.Model).ToArray();
    }
}