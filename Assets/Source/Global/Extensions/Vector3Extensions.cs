using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    private const float TrajectoryStep = 0.2f;

    public static Vector3 CalculateVelocity(this Vector3 forward, Vector3 direction, float angleRadian)
    {
        float gravity = Physics.gravity.y;
        float y = direction.y;
        float x = new Vector3(direction.x, (float)ValueConstants.Zero, direction.z).magnitude;
        return forward * Mathf.Sqrt(
            Mathf.Abs(
                gravity * x * x /
                ((float)ValueConstants.Two * (y - Mathf.Tan(angleRadian) * x) * Mathf.Cos(angleRadian) *
                 Mathf.Cos(angleRadian)))
        );
    }

    public static List<Vector3> CalculateTrajectory(
        this Vector3 forward,
        Vector3 currentPosition,
        Vector3 targetPosition,
        Vector3 direction,
        float angleRadian)
    {
        Vector3 velocity = forward.CalculateVelocity(direction, angleRadian);
        Vector3 gravity = Physics.gravity;
        Vector3 last = currentPosition;
        List<Vector3> result = new List<Vector3>();

        for (float i = 0f; i < direction.magnitude; i += TrajectoryStep)
        {
            Vector3 position = currentPosition + velocity * i + gravity * i * i / (float)ValueConstants.Two;

            if (currentPosition.y >= targetPosition.y && position.y < targetPosition.y ||
                currentPosition.y < targetPosition.y && position.y < targetPosition.y && last.y > targetPosition.y)
                break;

            result.Add(position);
            last = position;
        }

        return result;
    }

    public static Vector3 RotateAlongY(this Vector3 forward, Vector3 direction)
    {
        Vector3 newForward = Vector3.ClampMagnitude(
            new Vector3(direction.x, (float)ValueConstants.Zero, direction.z),
            new Vector3(forward.x, (float)ValueConstants.Zero, forward.z).magnitude);
        newForward.y = forward.y;
        return newForward;
    }
}