using UnityEngine;

namespace Localization
{
    public class LocalisationThrower : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}