using System;

namespace Assets.Source.Difficulty
{
    public class LevelConfiguration
    {
        private readonly int _militarySmallBuildings;
        private readonly int _militaryMediumBuildings;
        private readonly int _militaryLargeBuildings;
        private readonly int _obstacles;
        private readonly int _bunkers;

        public LevelConfiguration(int militarySmallBuildings, int militaryMediumBuildings, int militaryLargeBuildings, int obstacles, int bunkers)
        {
            _militarySmallBuildings = militarySmallBuildings >= 0 ? militarySmallBuildings : throw new ArgumentOutOfRangeException(nameof(militarySmallBuildings));
            _militaryMediumBuildings = militaryMediumBuildings >= 0 ? militaryMediumBuildings : throw new ArgumentOutOfRangeException(nameof(militaryMediumBuildings));
            _militaryLargeBuildings = militaryLargeBuildings >= 0 ? militaryLargeBuildings : throw new ArgumentOutOfRangeException(nameof(militaryLargeBuildings));
            _obstacles = obstacles >= 0 ? obstacles : throw new ArgumentOutOfRangeException(nameof(obstacles));
            _bunkers = bunkers >= 0 ? bunkers : throw new ArgumentOutOfRangeException(nameof(bunkers));
        }

        public int MilitarySmallBuildings => _militarySmallBuildings;

        public int MilitaryMediumBuildings => _militaryMediumBuildings;

        public int MilitaryLargeBuildings => _militaryLargeBuildings;

        public int Obstacles => _obstacles;

        public int Bunkers => _bunkers;
    }
}