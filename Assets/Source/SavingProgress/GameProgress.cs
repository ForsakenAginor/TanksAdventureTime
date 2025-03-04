using System;
using System.Linq;
using Shops;

namespace SavingProgress
{
    public class GameProgress
    {
        public int Level = 1;
        public int Currency = 0;
        public int Helper = 0;
        public int CompletedTrainingOnComputer = 0;
        public int CompletedTrainingOnMobile = 0;
        public bool HadHelper = false;
        public Purchases Purchases;

        public GameProgress()
        {
            Purchases = CreateStartPurchases();
        }

        private Purchases CreateStartPurchases()
        {
            return new Purchases(
                Enum.GetValues(typeof(GoodNames))
                    .Cast<GoodNames>()
                    .Select(type => new SerializedPair<GoodNames, int>(type, (int)ValueConstants.Zero))
                    .ToList());
        }
    }
}