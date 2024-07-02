using UnityEngine;

namespace Assets.Source.Localization
{
    public class LocalisationThrower : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}