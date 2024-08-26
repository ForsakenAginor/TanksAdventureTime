using UnityEngine;

namespace Assets.Source.DestructionObject
{
    public class MeshUnitor : MonoBehaviour
    {
        private void Start()
        {
            StaticBatchingUtility.Combine(gameObject);
        }
    }
}