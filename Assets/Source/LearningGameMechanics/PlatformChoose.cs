using UnityEngine;

public class PlatformChoose : MonoBehaviour
{
    [SerializeField] private Transform _mobileControllers;

    private void Awake()
    {
        if (Application.isMobilePlatform)
            _mobileControllers.gameObject.SetActive(true);
    }
}