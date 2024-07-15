using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Spawn(Vector3 position, GameObject spawned)
        {
            return Instantiate(spawned, position, Quaternion.identity);
        }
    }
}