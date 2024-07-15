using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class LevelGenerator
    {
        private readonly PointPresetCollection _presets;
        private readonly List<Vector3> _smallSpots;
        private readonly List<Vector3> _mediumSpots;
        private readonly List<Vector3> _largeSpots;
        private readonly LevelConfiguration _configuration;
        private readonly Spawner _spawner;
        private readonly IPlayerTarget _player;

        public LevelGenerator(
            LevelConfiguration configuration,
            PointPresetCollection presets,
            PointsSpotsCollection spots,
            Spawner spawner,
            IPlayerTarget player,
            Action<AudioSource> audioSourceAddedCallBack,
            Action<IEnumerable<Transform>> targetsSpawnedCallback)
        {
            _configuration = configuration != null ? configuration : throw new ArgumentNullException(nameof(configuration));
            _presets = presets != null ? presets : throw new ArgumentNullException(nameof(presets));
            _spawner = spawner != null ? spawner : throw new ArgumentNullException(nameof(spawner));
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));

            if (spots == null)
                throw new ArgumentNullException(nameof(spots));

            _smallSpots = spots.SmallBuildingSpots.ToList();
            _mediumSpots = spots.MediumBuildingSpots.ToList();
            _largeSpots = spots.LargeBuildingSpots.ToList();

            Generate(audioSourceAddedCallBack, targetsSpawnedCallback);
        }

        private void Generate(Action<AudioSource> audioSourceAddedCallBack, Action<IEnumerable<Transform>> targetsSpawnedCallback)
        {
            MilitaryPoint[] smallMilitaryPresets = ChoseMilitaryPresets(PointPresetSize.Small);
            MilitaryPoint[] mediumMilitaryPresets = ChoseMilitaryPresets(PointPresetSize.Medium);
            MilitaryPoint[] largeMilitaryPresets = ChoseMilitaryPresets(PointPresetSize.Large);
            CivilianPoint[] smallCivilianPresets = ChoseCivilianPresets(PointPresetSize.Small);
            CivilianPoint[] mediumCivilianPresets = ChoseCivilianPresets(PointPresetSize.Medium);
            CivilianPoint[] largeCivilianPresets = ChoseCivilianPresets(PointPresetSize.Large);

            if (_smallSpots.Count() < _configuration.MilitarySmallBuildings)
                throw new Exception("wrong level configuration: not enought small spots)");

            int amount = _configuration.MilitarySmallBuildings;
            InitializeMilitaryPoints(SpawnRandomBuildings(amount, smallMilitaryPresets, _smallSpots), audioSourceAddedCallBack, targetsSpawnedCallback);
            amount = _smallSpots.Count();
            SpawnRandomBuildings(amount, smallCivilianPresets, _smallSpots);

            if (_mediumSpots.Count() < _configuration.MilitaryMediumBuildings)
                throw new Exception("wrong level configuration: not enought medium spots)");

            amount = _configuration.MilitaryMediumBuildings;
            InitializeMilitaryPoints(SpawnRandomBuildings(amount, mediumMilitaryPresets, _mediumSpots), audioSourceAddedCallBack, targetsSpawnedCallback);
            amount = _mediumSpots.Count();
            SpawnRandomBuildings(amount, mediumCivilianPresets, _mediumSpots);

            if (_largeSpots.Count() < _configuration.MilitaryLargeBuildings)
                throw new Exception("wrong level configuration: not enought large spots)");

            amount = _configuration.MilitaryLargeBuildings;
            InitializeMilitaryPoints(SpawnRandomBuildings(amount, largeMilitaryPresets, _largeSpots), audioSourceAddedCallBack, targetsSpawnedCallback);
            amount = _largeSpots.Count();
            SpawnRandomBuildings(amount, largeCivilianPresets, _largeSpots);
        }

        private IEnumerable<Point> SpawnRandomBuildings(int amount, Point[] buildings, List<Vector3> spots)
        {
            List<Point> result = new();

            while (amount > 0)
            {
                int presetIndex = UnityEngine.Random.Range(0, buildings.Length);
                Vector3 spot = spots[UnityEngine.Random.Range(0, spots.Count)];
                result.Add(_spawner.Spawn(spot, buildings[presetIndex].gameObject).GetComponent<Point>());
                spots.Remove(spot);
                amount--;
            }

            return result;
        }

        private MilitaryPoint[] ChoseMilitaryPresets(PointPresetSize size)
        {
            return _presets.Presets.
                Where(o => o.Size == size && o.Prefab is MilitaryPoint).
                Select(o => o.Prefab as MilitaryPoint).
                ToArray();
        }

        private CivilianPoint[] ChoseCivilianPresets(PointPresetSize size)
        {
            return _presets.Presets.
                Where(o => o.Size == size && o.Prefab is CivilianPoint).
                Select(o => o.Prefab as CivilianPoint).
                ToArray();
        }

        private void InitializeMilitaryPoints(IEnumerable<Point> points, Action<AudioSource> audioSourceAddedCallBack, Action<IEnumerable<Transform>> targetsSpawnedCallback)
        {
            points.
                Select(o => o as MilitaryPoint).
                ToList().
                ForEach(o => o.Init(_player, audioSourceAddedCallBack, targetsSpawnedCallback));
        }
    }
}