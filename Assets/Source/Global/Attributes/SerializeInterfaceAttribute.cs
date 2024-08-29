#if UNITY_EDITOR
using System;
using UnityEngine;

public class SerializeInterfaceAttribute : PropertyAttribute
{
    public SerializeInterfaceAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; }
}
#endif