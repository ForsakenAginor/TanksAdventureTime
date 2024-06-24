using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void Spawn(Vector3 position, GameObject spawned)
    {
        Instantiate(spawned, position, Quaternion.identity);
    }
}
