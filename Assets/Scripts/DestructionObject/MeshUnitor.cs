using DestructionObject;
using System.Linq;
using UnityEngine;

public class MeshUnitor : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private void Start()
    {
        GetComponentsInChildren<Destruction>().ToList().ForEach(o => o.transform.parent = _parent);
        StaticBatchingUtility.Combine(_parent.gameObject);
    }
}
