using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class SpotMarker : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}