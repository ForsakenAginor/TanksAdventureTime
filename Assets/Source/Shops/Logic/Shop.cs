using System.Collections.Generic;
using System.Linq;

namespace Shops
{
    public class Shop
    {
        private readonly Dictionary<GoodNames, List<(object value, int price)>> _goodsContent;
        private readonly Purchases<int> _purchases;
        private readonly Purchases<object> _playerCharacteristics;

        public Shop(Dictionary<GoodNames, List<(object value, int price)>> goodsContent, Purchases<int> purchases)
        {
            _goodsContent = goodsContent;
            _purchases = purchases;
        }

        public IReadOnlyCharacteristics<int> GetPurchases()
        {
            return _purchases;
        }

        public bool TryGetPrice(GoodNames good, out int price)
        {
            int index = _purchases.Objects.First(item => item.Key == good).Value + 1;
            price = 0;

            if (index >= _goodsContent[good].Count)
                return false;

            price = _goodsContent[good][index].price;

            return true;
        }

        public bool TryGetNextPurchase(GoodNames good, out (object currentValue, object nextValue, int price) purchase)
        {
            purchase = (null, null, 0);
            int index = _purchases.Objects.First(item => item.Key == good).Value + 1;

            if (index >= _goodsContent[good].Count)
                return false;

            purchase = (_goodsContent[good][index - 1].value, _goodsContent[good][index].value,
                _goodsContent[good][index].price);

            return true;
        }

        public void Buy(GoodNames good)
        {
            SerializedPair<GoodNames, int> unpacked = _purchases.Objects.Find(item => item.Key == good);
            int id = _purchases.Objects.IndexOf(unpacked);
            unpacked.Value++;
            _purchases.Objects[id] = unpacked;
        }
    }
}