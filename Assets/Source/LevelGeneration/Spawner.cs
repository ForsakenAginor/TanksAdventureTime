using UnityEngine;

namespace LevelGeneration
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Spawn(Transform parent, GameObject spawned)
        {
            return Instantiate(spawned, parent);
        }
    }
}