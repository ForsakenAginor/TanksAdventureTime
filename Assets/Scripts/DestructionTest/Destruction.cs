using UnityEngine;

public class Destruction : MonoBehaviour
{
    [SerializeField] private Transform _panelDestruction;

    private Transform _transform;

    private void Start() => _transform = transform;

    public void DestroyObject()
    {
        _panelDestruction.transform.position = _transform.position;
        _panelDestruction.rotation = _transform.rotation;
        _panelDestruction.gameObject.SetActive(true);
        _transform.gameObject.SetActive(false);
    }
}