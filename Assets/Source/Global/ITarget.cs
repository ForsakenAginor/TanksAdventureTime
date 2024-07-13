using UnityEngine;

public interface ITarget
{
    public Vector3 Position { get; }

    public TargetPriority Priority { get; }
}