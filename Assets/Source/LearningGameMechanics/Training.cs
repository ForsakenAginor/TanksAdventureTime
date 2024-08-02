using UnityEngine.UI;

public abstract class Training : ImagePlatform
{
    private Image _currentImage;
    public bool IsPress { get; private set; } = false;

    public void TurnOn(bool isMobilePlatform)
    {
        _currentImage = GetImageCurrentPlatform(isMobilePlatform);
        _currentImage.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        _currentImage.gameObject.SetActive(false);
        IsPress = true;
    }
}