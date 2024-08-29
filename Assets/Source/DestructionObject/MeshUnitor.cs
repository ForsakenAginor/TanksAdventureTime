using UnityEngine;

namespace DestructionObject
{
    public class MeshUnitor : MonoBehaviour
    {
        private void Start()
        {
            StaticBatchingUtility.Combine(gameObject);
        }
    }
}