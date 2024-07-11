using UnityEngine;

namespace DestructionObject
{
    public class Ground : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Destruction destruction))
                destruction.DestroyObject();

            if (collision.gameObject.TryGetComponent(out DestroyedPart destroyed))
                destroyed.Die();
        }
    }
}