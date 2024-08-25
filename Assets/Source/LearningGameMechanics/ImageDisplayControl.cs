using UnityEngine;
using UnityEngine.UI;

public abstract class ImageDisplayControl : MonoBehaviour
{
    [SerializeField] private Image _imageMobilePlatform;

    public void TurnOn() => _imageMobilePlatform.gameObject.SetActive(true);

    public void TurnOff() => _imageMobilePlatform.gameObject.SetActive(false);
}