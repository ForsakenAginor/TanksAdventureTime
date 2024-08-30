using System;

namespace Difficulty
{
    public class LevelConfiguration
    {
        public LevelConfiguration(
            int militarySmallBuildings,
            int militaryMediumBuildings,
            int militaryLargeBuildings,
            int obstacles,
            int bunkers)
        {
            MilitarySmallBuildings = militarySmallBuildings >= 0
                ? militarySmallBuildings
                : throw new ArgumentOutOfRangeException(nameof(militarySmallBuildings));
            MilitaryMediumBuildings = militaryMediumBuildings >= 0
                ? militaryMediumBuildings
                : throw new ArgumentOutOfRangeException(nameof(militaryMediumBuildings));
            MilitaryLargeBuildings = militaryLargeBuildings >= 0
                ? militaryLargeBuildings
                : throw new ArgumentOutOfRangeException(nameof(militaryLargeBuildings));
            Obstacles = obstacles >= 0 ? obstacles : throw new ArgumentOutOfRangeException(nameof(obstacles));
            Bunkers = bunkers >= 0 ? bunkers : throw new ArgumentOutOfRangeException(nameof(bunkers));
        }

        public int MilitarySmallBuildings { get; }

        public int MilitaryMediumBuildings { get; }

        public int MilitaryLargeBuildings { get; }

        public int Obstacles { get; }

        public int Bunkers { get; }
    }
}