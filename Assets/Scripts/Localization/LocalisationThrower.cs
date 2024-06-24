using UnityEngine;

namespace Assets.Scripts.Core
{
    public class LocalisationThrower : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}