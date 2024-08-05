using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops
{
    [Serializable]
    public class Purchases : IReadOnlyCharacteristics
    {
        public List<SerializedPair<GoodNames, int>> Objects;

        public Purchases(List<SerializedPair<GoodNames, int>> stored)
        {
            Objects = stored;
        }

        public Dictionary<GoodNames, object> GetContent(Goods goods)
        {
            return goods.GetFormattedContent()
                .ToDictionary<KeyValuePair<GoodNames, List<(object value, int price)>>, GoodNames, object>(
                    pair => pair.Key,
                    pair => pair.Value[Objects.Find(item => item.Key == pair.Key).Value]);
        }
    }
}