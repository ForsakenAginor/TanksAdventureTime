using UnityEngine;

public class MeshUnitor : MonoBehaviour
{
    private void Start()
    {
        StaticBatchingUtility.Combine(gameObject);
    }
}
