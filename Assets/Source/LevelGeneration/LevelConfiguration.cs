using System;

namespace Assets.Source.LevelGeneration
{
    public class LevelConfiguration
    {
        private readonly int _militarySmallBuildings;
        private readonly int _militaryMediumBuildings;
        private readonly int _militaryLargeBuildings;

        public LevelConfiguration(int militarySmallBuildings, int militaryMediumBuildings, int militaryLargeBuildings)
        {
            _militarySmallBuildings = militarySmallBuildings >= 0 ? militarySmallBuildings : throw new ArgumentOutOfRangeException(nameof(militarySmallBuildings));
            _militaryMediumBuildings = militaryMediumBuildings >= 0 ? militaryMediumBuildings : throw new ArgumentOutOfRangeException(nameof(militaryMediumBuildings));
            _militaryLargeBuildings = militaryLargeBuildings >= 0 ? militaryLargeBuildings : throw new ArgumentOutOfRangeException(nameof(militaryLargeBuildings));
        }

        public int MilitarySmallBuildings => _militarySmallBuildings;

        public int MilitaryMediumBuildings => _militaryMediumBuildings;

        public int MilitaryLargeBuildings => _militaryLargeBuildings;
    }
}