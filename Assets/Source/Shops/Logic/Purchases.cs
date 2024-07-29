using System;
using System.Collections.Generic;

namespace Shops
{
    [Serializable]
    public class Purchases<T> : IReadOnlyCharacteristics<T>
    {
        public List<SerializedPair<GoodNames, T>> Objects;
        public Purchases(List<SerializedPair<GoodNames, T>> stored)
        {
            Objects = stored;
        }

        public IReadOnlyList<SerializedPair<GoodNames, T>> Content => Objects;
    }
}