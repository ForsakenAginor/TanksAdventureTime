using UnityEngine;

namespace DestructionObject
{
    public class DestroyedPart : MonoBehaviour, IPermanentKiller
    {
        private const string DieObject = "DisableObject";

        public void Die()
        {
            float minValue = 1f;
            float maxValue = 5f;
            float timeDie = Random.Range(minValue, maxValue);
            Invoke(DieObject, timeDie);
        }

        private void DisableObject() => gameObject.SetActive(false);
    }
}