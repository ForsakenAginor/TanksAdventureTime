using System.Collections.Generic;

namespace Shops
{
    public interface IReadOnlyCharacteristics
    {
        public Dictionary<GoodNames, object> GetContent(Goods goods);
    }
}