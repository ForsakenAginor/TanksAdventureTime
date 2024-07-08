using Projectiles;
using UnityEngine;

public class PlayerWeapon : IWeapon
{
    private readonly IProjectileFactory Factory;
    private readonly Transform ShootPoint;
    private readonly Transform Transform;
    private readonly LineRenderer Line;
    private readonly float MaxDistance;
    private readonly float AngleRadian;

    public PlayerWeapon(
        IProjectileFactory factory,
        Transform shootPoint,
        Transform transform,
        LineRenderer line,
        float maxDistance,
        float angleRadian)
    {
        Factory = factory;
        ShootPoint = shootPoint;
        Transform = transform;
        Line = line;
        MaxDistance = maxDistance;
        AngleRadian = angleRadian;
    }

    public void Shoot()
    {
        Vector3 forward = ShootPoint.forward;
        Vector3 currentPosition = ShootPoint.position;
        Vector3 targetPosition = CalculateTargetPosition(forward);
        Factory.Create(currentPosition, targetPosition, targetPosition - currentPosition, forward);
    }

    public void StartDrawTrajectory()
    {
        Line.enabled = true;
    }

    public void StopDrawTrajectory()
    {
        Line.enabled = false;
    }

    public void DrawTrajectory()
    {
        if (Line.enabled == false)
            return;

        Vector3 forward = ShootPoint.forward;
        Vector3 currentPosition = ShootPoint.position;
        Vector3 targetPosition = CalculateTargetPosition(forward);
        Line.SetPositions(
            forward.CalculateTrajectory(
                currentPosition,
                targetPosition,
                targetPosition - currentPosition,
                AngleRadian).ToArray());
    }

    private Vector3 CalculateTargetPosition(Vector3 forward)
    {
        Vector3 result = forward * MaxDistance;
        result.y = Transform.position.y;
        return result;
    }
}