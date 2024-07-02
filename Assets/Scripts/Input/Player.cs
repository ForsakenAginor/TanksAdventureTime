using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MovingSystem _movingSystem;
    private AimSystem _aimSystem;

    private void FixedUpdate()
    {
        if (_movingSystem == null || _aimSystem == null)
            return;

        _movingSystem.Moving();
        _aimSystem.Aim();
    }

    public void Init(MovingSystem movingSystem, AimSystem aimSystem)
    {
        _movingSystem = movingSystem != null ? movingSystem : throw new ArgumentNullException(nameof(movingSystem));
        _aimSystem = aimSystem != null ? aimSystem : throw new ArgumentNullException(nameof(aimSystem));
    }
}
