using System;
using Unity.VisualScripting;
using UnityEngine;

public class BunkerPart : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _destruction;

    private DestroyedPartBunker[] _buncerDestructionObjects;
    private Transform _transform;

    public event Action Died;
    public event Action<Action> Destructed;

    private void Awake()
    {
        Init();
        _transform = transform;
    }

    public void TakeDamage(int value)
    {
        React();
        Destructed?.Invoke(Died);
    }

    private void React()
    {
        _destruction.position = _transform.position;
        _destruction.rotation = _transform.rotation;
        _destruction.gameObject.SetActive(true);
        gameObject.SetActive(false);

        for (int i = 0; i < _buncerDestructionObjects.Length; i++)
            _buncerDestructionObjects[i].React(_transform);
    }

    private void Init()
    {
        _buncerDestructionObjects = new DestroyedPartBunker[_destruction.childCount];

        for (int i = 0; i < _buncerDestructionObjects.Length; i++)
            _buncerDestructionObjects[i] = _destruction.GetChild(i).AddComponent<DestroyedPartBunker>();
    }
}