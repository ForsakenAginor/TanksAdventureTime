using UnityEngine;
using UnityEngine.UI;

public abstract class ImagePlatform : MonoBehaviour
{
    [SerializeField] private Image _imageMobilePlatform;
    [SerializeField] private Image _imageComputerPlatform;

    public Image GetImageCurrentPlatform(bool isMobilePlatform)
    {
        if (isMobilePlatform == true)
            return _imageMobilePlatform;

        return _imageComputerPlatform;
    }
}
