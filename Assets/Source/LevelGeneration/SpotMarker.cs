using UnityEngine;

namespace LevelGeneration
{
    public class SpotMarker : MonoBehaviour
    {
        [SerializeField] private Vector3 _cube;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, _cube);
        }
    }
}