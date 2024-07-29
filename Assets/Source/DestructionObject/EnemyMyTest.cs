using UnityEngine;

namespace DestructionObject
{
    public class EnemyMyTest : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IPermanentKiller permanentKiller))
                if (gameObject.TryGetComponent(out Rigidbody rigidbody) == false)
                    gameObject.AddComponent<Rigidbody>();
        }

        public void AddRigidbody() => this.gameObject.AddComponent<Rigidbody>();
    }
}