using UnityEngine;

public interface IPlayerTarget : IDamageableTarget
{
    public Vector3 GetClosestPoint(Vector3 position);
}