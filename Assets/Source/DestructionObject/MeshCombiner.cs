using UnityEngine;

namespace DestructionObject
{
    public class MeshCombiner : MonoBehaviour
    {
        private void Start()
        {
            StaticBatchingUtility.Combine(gameObject);
        }
    }
}