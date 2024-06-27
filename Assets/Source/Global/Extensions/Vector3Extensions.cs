using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 CalculateVelocity(this Vector3 forward, Vector3 direction, float angleRadian, float gravity)
    {
        float y = direction.y;
        float x = new Vector3(direction.x, (float)ValueConstants.Zero, direction.z).magnitude;
        return forward * Mathf.Sqrt(
            Mathf.Abs(
                gravity * x * x /
                ((float)ValueConstants.Two * (y - Mathf.Tan(angleRadian) * x) * Mathf.Cos(angleRadian) * Mathf.Cos(angleRadian)))
        );
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