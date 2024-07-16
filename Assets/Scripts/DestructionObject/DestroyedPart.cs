using UnityEngine;

namespace DestructionObject
{
    public class DestroyedPart : MonoBehaviour, IPermanentKiller, IReactive
    {
        private const string DieObject = "DisableObject";

        public void React()
        {
            float minValue = 1f;
            float maxValue = 5f;
            float timeDie = Random.Range(minValue, maxValue);
           // Invoke(DieObject, timeDie);
        }

        private void DisableObject() => gameObject.SetActive(false);
    }
}