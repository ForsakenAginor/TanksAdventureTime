using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shops
{
    [CreateAssetMenu(fileName = "Goods", menuName = "Goods", order = 3)]
    public class Goods : UpdatableConfiguration<GoodNames,
        SerializedPair<GoodsValues, List<SerializedPair<MixedValue, int>>>>
    {
        [SerializeField] private List<SerializedPair<GoodNames, Sprite>> _icons = new ();

        public override void OnEndValidate()
        {
            UpdateContent(_icons);
        }

        public Dictionary<GoodNames, List<(object value, int price)>> GetContent()
        {
            Dictionary<GoodNames, List<(object value, int price)>> result = new ();

            foreach (var serializedPair in Content)
            {
                GoodNames good = serializedPair.Key;
                GoodsValues goodValue = serializedPair.Value.Key;
                List<(object value, int price)> purchase = new ();

                foreach (SerializedPair<MixedValue, int> pair in serializedPair.Value.Value)
                    purchase.Add((pair.Key.GetValue(goodValue), pair.Value));

                result.Add(good, purchase);
            }

            return result;
        }

        public Dictionary<GoodNames, Sprite> GetIcons()
        {
            return _icons.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}