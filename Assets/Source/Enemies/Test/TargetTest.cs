using UnityEngine;

public class TargetTest : MonoBehaviour, ITarget
{
    private Transform _transform;

    public Vector3 Position => _transform.position;

    private void Awake()
    {
        _transform = transform;
    }
}