using System;
using Shops;
using System.Linq;

public class GameData
{
    public int Level = 1;
    public int Currency = 0;
    public int Helper = 0;
    public int CompletedTraining = 0;
    public bool HadHelper = false;
    public Purchases Purchases;

    public GameData()
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