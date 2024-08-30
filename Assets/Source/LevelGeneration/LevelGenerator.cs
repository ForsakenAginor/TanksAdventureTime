using System;
using System.Collections.Generic;
using System.Linq;
using Difficulty;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGeneration
{
    public class LevelGenerator
    {
        private const string ConfigSmallMessage = "Wrong level configuration: not enough small spots";
        private const string ConfigMediumMessage = "Wrong level configuration: not enough medium spots";
        private const string ConfigLargeMessage = "Wrong level configuration: not enough large spots";
        private const string ConfigObstaclesMessage = "Wrong level configuration: not enough obstacles spots";

        private readonly PointPresetCollection _presets;
        private readonly List<Transform> _smallSpots;
        private readonly List<Transform> _mediumSpots;
        private readonly List<Transform> _largeSpots;
        private readonly List<Transform> _obstaclesSpots;
        private readonly List<Transform> _bunkerSpots;
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
            Action<IDamageableTarget> targetSpawnedCallback)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _presets = presets != null ? presets : throw new ArgumentNullException(nameof(presets));
            _spawner = spawner != null ? spawner : throw new ArgumentNullException(nameof(spawner));
            _player = player ?? throw new ArgumentNullException(nameof(player));

            if (spots == null)
                throw new ArgumentNullException(nameof(spots));

            _smallSpots = spots.SmallBuildingSpots.ToList();
            _mediumSpots = spots.MediumBuildingSpots.ToList();
            _largeSpots = spots.LargeBuildingSpots.ToList();
            _obstaclesSpots = spots.ObstaclesSpots.ToList();
            _bunkerSpots = spots.BunkerSpots.ToList();

            Generate(audioSourceAddedCallBack, targetSpawnedCallback);
        }

        private void Generate(
            Action<AudioSource> audioSourceAddedCallBack,
            Action<IDamageableTarget> targetSpawnedCallback)
        {
            MilitaryPoint[] smallMilitaryPresets = ChoseMilitaryPresets(PointPresetSize.Small);
            MilitaryPoint[] mediumMilitaryPresets = ChoseMilitaryPresets(PointPresetSize.Medium);
            MilitaryPoint[] largeMilitaryPresets = ChoseMilitaryPresets(PointPresetSize.Large);
            CivilianPoint[] smallCivilianPresets = ChoseCivilianPresets(PointPresetSize.Small);
            CivilianPoint[] mediumCivilianPresets = ChoseCivilianPresets(PointPresetSize.Medium);
            CivilianPoint[] largeCivilianPresets = ChoseCivilianPresets(PointPresetSize.Large);
            ObstaclesPoint[] obstaclesPresets = ChoseObstaclesPresets();
            BunkerPoint[] bunkerPresets = ChoseBunkerPresets();

            if (_smallSpots.Count < _configuration.MilitarySmallBuildings)
                throw new Exception(ConfigSmallMessage);

            int amount = _configuration.MilitarySmallBuildings;
            InitializeMilitaryPoints(
                SpawnRandomBuildings(amount, smallMilitaryPresets, _smallSpots),
                audioSourceAddedCallBack,
                targetSpawnedCallback);
            amount = _smallSpots.Count;
            SpawnRandomBuildings(amount, smallCivilianPresets, _smallSpots);

            if (_mediumSpots.Count < _configuration.MilitaryMediumBuildings)
                throw new Exception(ConfigMediumMessage);

            amount = _configuration.MilitaryMediumBuildings;
            InitializeMilitaryPoints(
                SpawnRandomBuildings(amount, mediumMilitaryPresets, _mediumSpots),
                audioSourceAddedCallBack,
                targetSpawnedCallback);
            amount = _mediumSpots.Count;
            SpawnRandomBuildings(amount, mediumCivilianPresets, _mediumSpots);

            if (_largeSpots.Count < _configuration.MilitaryLargeBuildings)
                throw new Exception(ConfigLargeMessage);

            amount = _configuration.MilitaryLargeBuildings;
            InitializeMilitaryPoints(
                SpawnRandomBuildings(amount, largeMilitaryPresets, _largeSpots),
                audioSourceAddedCallBack,
                targetSpawnedCallback);
            amount = _largeSpots.Count;
            SpawnRandomBuildings(amount, largeCivilianPresets, _largeSpots);

            if (_obstaclesSpots.Count < _configuration.Obstacles)
                throw new Exception(ConfigObstaclesMessage);

            amount = _configuration.Obstacles;
            InitializeObstaclesPoints(
                SpawnRandomBuildings(amount, obstaclesPresets, _obstaclesSpots),
                audioSourceAddedCallBack);

            amount = _configuration.Bunkers;
            InitializeBunkerPoints(
                SpawnRandomBuildings(amount, bunkerPresets, _bunkerSpots),
                audioSourceAddedCallBack,
                targetSpawnedCallback);
        }

        private IEnumerable<Point> SpawnRandomBuildings(int amount, Point[] buildings, List<Transform> spots)
        {
            List<Point> result = new ();

            while (amount > 0)
            {
                int presetIndex = Random.Range(0, buildings.Length);
                Transform spot = spots[Random.Range(0, spots.Count)];
                result.Add(_spawner.Spawn(spot, buildings[presetIndex].gameObject).GetComponent<Point>());
                spots.Remove(spot);
                amount--;
            }

            return result;
        }

        private MilitaryPoint[] ChoseMilitaryPresets(PointPresetSize size)
        {
            return _presets.Presets
                .Where(o => o.Size == size && o.Prefab is MilitaryPoint)
                .Select(o => o.Prefab as MilitaryPoint)
                .ToArray();
        }

        private CivilianPoint[] ChoseCivilianPresets(PointPresetSize size)
        {
            return _presets.Presets
                .Where(o => o.Size == size && o.Prefab is CivilianPoint)
                .Select(o => o.Prefab as CivilianPoint)
                .ToArray();
        }

        private ObstaclesPoint[] ChoseObstaclesPresets()
        {
            return _presets.Presets
                .Where(o => o.Prefab is ObstaclesPoint)
                .Select(o => o.Prefab as ObstaclesPoint)
                .ToArray();
        }

        private BunkerPoint[] ChoseBunkerPresets()
        {
            return _presets.Presets
                .Where(o => o.Prefab is BunkerPoint)
                .Select(o => o.Prefab as BunkerPoint)
                .ToArray();
        }

        private void InitializeMilitaryPoints(
            IEnumerable<Point> points,
            Action<AudioSource> audioSourceAddedCallBack,
            Action<IDamageableTarget> targetSpawnedCallback)
        {
            points
                .Select(o => o as MilitaryPoint)
                .ToList()
                .ForEach(o => o.Init(_player, audioSourceAddedCallBack, targetSpawnedCallback));
        }

        private void InitializeObstaclesPoints(IEnumerable<Point> points, Action<AudioSource> audioSourceAddedCallBack)
        {
            points
                .Select(o => o as ObstaclesPoint)
                .ToList()
                .ForEach(o => o.Init(_player, audioSourceAddedCallBack));
        }

        private void InitializeBunkerPoints(
            IEnumerable<Point> points,
            Action<AudioSource> audioSourceAddedCallBack,
            Action<IDamageableTarget> targetSpawnedCallback)
        {
            points
                .Select(o => o as BunkerPoint)
                .ToList()
                .ForEach(o => o.Init(_player, audioSourceAddedCallBack, targetSpawnedCallback));
        }
    }
}