using System.Collections.Generic;

namespace Shops
{
    public interface IReadOnlyCharacteristics<T>
    {
        public IReadOnlyList<SerializedPair<GoodNames, T>> Content { get; }
    }
}