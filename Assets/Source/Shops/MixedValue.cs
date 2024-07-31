using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shops
{
    [Serializable]
    public class MixedValue
    {
        [SerializeField] private int _intValue;
        [SerializeField] private float _floatValue;
        [SerializeField] private bool _boolValue;

        public object GetValue(GoodsValues valueType)
        {
            switch (valueType)
            {
                case GoodsValues.Int:
                    return _intValue;

                case GoodsValues.Float:
                    return _floatValue;

                case GoodsValues.Bool:
                    return _boolValue;

                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }
        }
    }
}