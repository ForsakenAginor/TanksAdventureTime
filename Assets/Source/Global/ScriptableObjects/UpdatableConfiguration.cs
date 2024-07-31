using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UpdatableConfiguration<TK, TV> : ScriptableObject
    where TK : Enum
{
    [SerializeField] private List<SerializedPair<TK, TV>> _content;

    public IReadOnlyList<SerializedPair<TK, TV>> Content => _content;

    private void OnValidate()
    {
        foreach (var pair in Create().Where(pair => _content.Exists(item => Equals(item.Key, pair.Key)) == false))
            _content.Add(pair);

        OnValidateEnd();
    }

    public virtual void OnValidateEnd()
    {
    }

    private List<SerializedPair<TK, TV>> Create()
    {
        List<SerializedPair<TK, TV>> result = new ();
        TV value = default;

        foreach (TK type in Enum.GetValues(typeof(TK)))
            result.Add(new SerializedPair<TK, TV>(type, value));

        return result;
    }
}