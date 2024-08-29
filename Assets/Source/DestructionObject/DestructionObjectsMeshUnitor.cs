using System.Linq;
using UnityEngine;

namespace DestructionObject
{
    public class DestructionObjectsMeshUnitor : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        private void Start()
        {
            GetComponentsInChildren<Destruction>().ToList().ForEach(o => o.transform.parent = _parent);
            StaticBatchingUtility.Combine(_parent.gameObject);
        }
    }
}