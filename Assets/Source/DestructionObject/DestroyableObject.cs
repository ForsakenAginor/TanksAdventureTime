using UnityEngine;

namespace DestructionObject
{
    public class DestroyableObject : MonoBehaviour, IReactive
    {
        public void React()
        {
            gameObject.SetActive(false);
        }
    }
}