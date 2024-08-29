using System;

namespace Difficulty
{
    public class DifficultySystem
    {
        private const float SmallScaleFactor = 0.5f;
        private const float MediumScaleFactor = 0.5f;
        private const float LargeScaleFactor = 0.2f;
        private const float ObstaclesScaleFactor = 0.25f;
        private const float BunkersScaleFactor = 0.25f;

        private readonly LevelConfiguration _minConfiguration = new (3, 1, 0, 0, 0);
        private readonly LevelConfiguration _maxConfiguration = new (10, 10, 2, 4, 2);

        public DifficultySystem(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            level--;

            int smallSpots = Math.Min(
                _minConfiguration.MilitarySmallBuildings + (int)(level * SmallScaleFactor),
                _maxConfiguration.MilitarySmallBuildings);
            int mediumSpots = Math.Min(
                _minConfiguration.MilitaryMediumBuildings + (int)(level * MediumScaleFactor),
                _maxConfiguration.MilitaryMediumBuildings);
            int largeSpots = Math.Min(
                _minConfiguration.MilitaryLargeBuildings + (int)(level * LargeScaleFactor),
                _maxConfiguration.MilitaryLargeBuildings);
            int obstacles = Math.Min(
                _minConfiguration.Obstacles + (int)(level * ObstaclesScaleFactor),
                _maxConfiguration.Obstacles);
            int bunkers = Math.Min(
                _minConfiguration.Bunkers + (int)(level * BunkersScaleFactor),
                _maxConfiguration.Bunkers);

            CurrentConfiguration = new LevelConfiguration(smallSpots, mediumSpots, largeSpots, obstacles, bunkers);
        }

        public LevelConfiguration CurrentConfiguration { get; }
    }
}