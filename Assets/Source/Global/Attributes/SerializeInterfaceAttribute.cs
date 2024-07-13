using System;
using UnityEngine;

public class SerializeInterfaceAttribute : PropertyAttribute
{
    public Type Type { get; }

    public SerializeInterfaceAttribute(Type type)
    {
        Type = type;
    }
}