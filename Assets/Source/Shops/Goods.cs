using System.Collections.Generic;
using UnityEngine;

namespace Shops
{
    [CreateAssetMenu(fileName = "Goods", menuName = "Goods", order = 3)]
    public class Goods : UpdatableConfiguration<GoodNames,
        SerializedPair<GoodsValues, List<SerializedPair<MixedValue, int>>>>
    {
        public override void OnValidateEnd()
        {
            Debug.Log(GetContent().Count);
            List<SerializedPair<GoodNames, int>> values = new ()
            {
                new (GoodNames.AmmoCount, 0),
                new (GoodNames.ReloadSpeed, 0),
            };
            IReadOnlyCharacteristics<int> purchases = new Purchases<int>(values);
            Debug.Log(JsonUtility.ToJson(purchases));
            Debug.Log(JsonUtility.FromJson<Purchases<int>>(JsonUtility.ToJson(purchases)).Content.Count);
        }

        public Dictionary<GoodNames, List<(object value, int price)>> GetContent()
        {
            Dictionary<GoodNames, List<(object value, int price)>> content = new ();

            foreach (var serializedPair in Content)
            {
                GoodNames good = serializedPair.Key;
                GoodsValues goodValue = serializedPair.Value.Key;
                List<(object value, int price)> result = new ();

                foreach (SerializedPair<MixedValue, int> pair in serializedPair.Value.Value)
                    result.Add((pair.Key.GetValue(goodValue), pair.Value));

                content.Add(good, result);
            }

            return content;
        }
    }
}