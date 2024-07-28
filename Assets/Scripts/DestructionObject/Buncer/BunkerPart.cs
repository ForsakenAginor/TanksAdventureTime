using Unity.VisualScripting;
using UnityEngine;

public class BunkerPart : MonoBehaviour
{
    [SerializeField] private Transform _destruction;

    private DestroyedPartBunker[] _buncerDestructionObjects;
    private Transform _transform;

    public bool IsDestroyed { get; private set; } = false;

    private void Awake() => Init();

    public void React()
    {
        _destruction.position = _transform.position;
        _destruction.rotation = _transform.rotation;
        _destruction.gameObject.SetActive(true);
        gameObject.SetActive(false);

        for (int i = 0; i < _buncerDestructionObjects.Length; i++)
            _buncerDestructionObjects[i].React(_transform);

        IsDestroyed = true;
    }

    private void Init()
    {
        _transform = transform;
        _buncerDestructionObjects = new DestroyedPartBunker[_destruction.childCount];

        for (int i = 0; i < _buncerDestructionObjects.Length; i++)
            _buncerDestructionObjects[i] = _destruction.GetChild(i).AddComponent<DestroyedPartBunker>();
    }
}