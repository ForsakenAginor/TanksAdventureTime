using System;
using System.Collections.Generic;

[Serializable]
public struct SerializedPair<TK, TV>
{
    public TK Key;
    public TV Value;

    public SerializedPair(TK key, TV value)
    {
        Key = key;
        Value = value;
    }

    public static bool operator ==(SerializedPair<TK, TV> first, SerializedPair<TK, TV> second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(SerializedPair<TK, TV> first, SerializedPair<TK, TV> second)
    {
        return first.Equals(second) == false;
    }

    public bool Equals(SerializedPair<TK, TV> other)
    {
        return EqualityComparer<TK>.Default.Equals(Key, other.Key) &&
               EqualityComparer<TV>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object @object)
    {
        return @object is SerializedPair<TK, TV> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Value);
    }
}