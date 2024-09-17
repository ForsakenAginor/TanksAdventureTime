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
            Dictionary<Difficulty.Point, Point[]> dictionary = new Dictionary<Difficulty.Point, Point[]>()
            {
                {Difficulty.Point.Small, ChoseMilitaryPresets(PointPresetSize.Small) },
                {Difficulty.Point.Medium, ChoseMilitaryPresets(PointPresetSize.Medium)},
                {Difficulty.Point.Large, ChoseMilitaryPresets(PointPresetSize.Large) },
                {Difficulty.Point.Obstacle, ChoseObstaclesPresets()},
                {Difficulty.Point.Bunker, ChoseBunkerPresets() },
            };

            CivilianPoint[] smallCivilianPresets = ChoseCivilianPresets(PointPresetSize.Small);
            CivilianPoint[] mediumCivilianPresets = ChoseCivilianPresets(PointPresetSize.Medium);
            CivilianPoint[] largeCivilianPresets = ChoseCivilianPresets(PointPresetSize.Large);

            GenerateMilitaryPoints(Difficulty.Point.Small, _smallSpots, dictionary[Difficulty.Point.Small],
                audioSourceAddedCallBack, targetSpawnedCallback);
            SpawnRandomBuildings(_smallSpots.Count, smallCivilianPresets, _smallSpots);

            GenerateMilitaryPoints(Difficulty.Point.Medium, _mediumSpots, dictionary[Difficulty.Point.Medium],
                audioSourceAddedCallBack, targetSpawnedCallback);
            SpawnRandomBuildings(_mediumSpots.Count, mediumCivilianPresets, _mediumSpots);

            GenerateMilitaryPoints(Difficulty.Point.Large, _largeSpots, dictionary[Difficulty.Point.Large],
                audioSourceAddedCallBack, targetSpawnedCallback);
            SpawnRandomBuildings(_largeSpots.Count, largeCivilianPresets, _largeSpots);

            GenerateMilitaryPoints(Difficulty.Point.Obstacle, _obstaclesSpots, dictionary[Difficulty.Point.Obstacle],
                audioSourceAddedCallBack, targetSpawnedCallback);

            GenerateMilitaryPoints(Difficulty.Point.Bunker, _bunkerSpots, dictionary[Difficulty.Point.Bunker],
                audioSourceAddedCallBack, targetSpawnedCallback);
        }

        private void GenerateMilitaryPoints(Difficulty.Point point,
            List<Transform> spots,
            Point[] presets,
            Action<AudioSource> audioSourceAddedCallBack,
            Action<IDamageableTarget> targetSpawnedCallback)
        {
            int amount = _configuration.PointsAmount[point];

            if (spots.Count < amount)
                throw new Exception($"Wrong level configuration: not enough {point} spots");

            InitializeMilitaryPoints(
                SpawnRandomBuildings(amount, presets, spots),
                audioSourceAddedCallBack,
                targetSpawnedCallback);
        }

        private IEnumerable<Point> SpawnRandomBuildings(int amount, Point[] buildings, List<Transform> spots)
        {
            List<Point> result = new();

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
    }
}