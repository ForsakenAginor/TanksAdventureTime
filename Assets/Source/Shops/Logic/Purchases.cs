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
            Dictionary<GoodNames, object> result = new ();

            foreach (KeyValuePair<GoodNames, List<(object value, int price)>> pair in goods.GetFormattedContent())
            {
                int id = Objects.Find(item => item.Key == pair.Key).Value;
                result.Add(pair.Key, pair.Value[id].value);
            }

            return result;
        }
    }
}