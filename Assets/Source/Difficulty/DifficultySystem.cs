using System;

namespace Assets.Source.Difficulty
{
    public class DifficultySystem
    {
        private readonly LevelConfiguration _minConfiguration = new(3, 1, 0, 0);
        private readonly LevelConfiguration _maxConfiguration = new(10, 10, 2, 4);
        private readonly float _smallScaleFactor = 0.5f;
        private readonly float _mediumScaleFactor = 0.5f;
        private readonly float _largecaleFactor = 0.2f;
        private readonly float _obstaclesScaleFactor = 0.25f;

        private readonly LevelConfiguration _currentConfiguration;

        public DifficultySystem(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            level--;

            int smallSpots = Math.Min(_minConfiguration.MilitarySmallBuildings + (int)(level * _smallScaleFactor),
                                        _maxConfiguration.MilitarySmallBuildings);
            int mediumSpots = Math.Min(_minConfiguration.MilitaryMediumBuildings + (int)(level * _mediumScaleFactor),
                                        _maxConfiguration.MilitaryMediumBuildings);
            int largeSpots = Math.Min(_minConfiguration.MilitaryLargeBuildings + (int)(level * _largecaleFactor),
                                        _maxConfiguration.MilitaryLargeBuildings);
            int obstacles = Math.Min(_minConfiguration.Obstacles + (int)(level * _obstaclesScaleFactor),
                                        _maxConfiguration.Obstacles);

            _currentConfiguration = new(smallSpots, mediumSpots, largeSpots, obstacles);
        }

        public LevelConfiguration CurrentConfiguration => _currentConfiguration;
    }
}