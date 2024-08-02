using Shops;
using System.Collections.Generic;

public class GameData
{
    public int Level = 1;
    public int Currency = 0;
    public int Helper = 0;
    public int CompletedTraining = 0;
    public Purchases<int> Purchases = new(new List<SerializedPair<GoodNames, int>>());
}
