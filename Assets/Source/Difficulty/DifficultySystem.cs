using System;
using System.Collections.Generic;

namespace Difficulty
{
    public partial class DifficultySystem
    {
        private readonly LevelConfiguration _minConfiguration = new (3, 1, 0, 0, 0);
        private readonly LevelConfiguration _maxConfiguration = new (10, 10, 2, 4, 2);
        private readonly Dictionary<Point, float> _pointsScaleFactors = new Dictionary<Point, float>()
        {
            {Point.Small, 0.5f },
            {Point.Medium, 0.5f },
            {Point.Large, 0.2f },
            {Point.Obstacle, 0.25f },
            {Point.Bunker, 0.25f },
        };

        public DifficultySystem(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            level--;

            int smallSpots = GetAmountOfPoints(level, Point.Small);
            int mediumSpots = GetAmountOfPoints(level, Point.Medium);
            int largeSpots = GetAmountOfPoints(level, Point.Large);
            int obstacles = GetAmountOfPoints(level, Point.Obstacle);
            int bunkers = GetAmountOfPoints(level, Point.Bunker);

            CurrentConfiguration = new LevelConfiguration(smallSpots, mediumSpots, largeSpots, obstacles, bunkers);
        }

        public LevelConfiguration CurrentConfiguration { get; }

        private int GetAmountOfPoints(int level, Point point)
        {
            return Math.Min(
                _minConfiguration.PointsAmount[point] + (int)(level * _pointsScaleFactors[point]),
                _maxConfiguration.PointsAmount[point]);
        }
    }
}