using UnityEngine;

namespace DestructionObject
{
    public class EnemyTony : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IPermanentKiller permanentKiller))
            {
                if (gameObject.TryGetComponent(out Rigidbody rigidbody) == false)
                {
                    Debug.Log("DDW");
                    gameObject.AddComponent<Rigidbody>();
                }
            }
        }
    }
}
