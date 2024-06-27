using Enemies;
using UnityEngine;

public interface IPlayerTarget : ITarget
{
    public Vector3 GetClosestPoint(Vector3 position);

    public void TakeHit(HitTypes type);
}