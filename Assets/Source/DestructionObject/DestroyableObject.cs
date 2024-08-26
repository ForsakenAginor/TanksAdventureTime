using UnityEngine;

namespace Assets.Source.DestructionObject
{
    public class DestroyableObject : MonoBehaviour, IReactive
    {
        public void React()
        {
            gameObject.SetActive(false);
        }
    }
}