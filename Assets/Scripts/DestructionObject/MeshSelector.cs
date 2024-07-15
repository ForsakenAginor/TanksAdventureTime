using DestructionObject;
using System.Linq;
using UnityEngine;

public class MeshSelector : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private void Start()
    {
        GetComponentsInChildren<Destruction>().ToList().ForEach(o =>
        {
           // o.gameObject.isStatic = true;
            o.transform.parent = _parent;
        });

        StaticBatchingUtility.Combine(_parent.gameObject);
    }
}
