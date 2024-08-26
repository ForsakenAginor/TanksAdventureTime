using UnityEngine;

namespace Assets.Source.DestructionObject
{
    public class Ground : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IReactive reactive))
                reactive.React();
        }
    }
}