using System;
using System.Collections.Generic;

namespace Difficulty
{
    public class LevelConfiguration
    {
        private readonly Dictionary<Point, int> _pointsAmount = new Dictionary<Point, int>();

        public LevelConfiguration(
            int militarySmallBuildings,
            int militaryMediumBuildings,
            int militaryLargeBuildings,
            int obstacles,
            int bunkers)
        {
            AddPointsAmount(militarySmallBuildings, Point.Small);
            AddPointsAmount(militaryMediumBuildings, Point.Medium);
            AddPointsAmount(militaryLargeBuildings, Point.Large);
            AddPointsAmount(obstacles, Point.Obstacle);
            AddPointsAmount(bunkers, Point.Bunker);
        }

        public IReadOnlyDictionary<Point, int> PointsAmount => _pointsAmount;

        private void AddPointsAmount(int amount, Point point)
        {
            if (amount >= 0)
                _pointsAmount.Add(point, amount);
            else
                throw new ArgumentOutOfRangeException(nameof(amount));
        }
    }
}