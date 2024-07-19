using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Spawn(Transform parent, GameObject spawned)
        {
            return Instantiate(spawned, parent);
        }
    }
}