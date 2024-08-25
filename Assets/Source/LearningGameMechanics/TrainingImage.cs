using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class TrainingImage : MonoBehaviour
{
    [SerializeField] private Image _imageMobilePlatform;
    [SerializeField] private Transform _description;

    public Transform ImageTransform => _imageMobilePlatform.transform;

    private void Start() => TrainingStart();

    protected abstract void TrainingStart();

    public void TurnOn()
    {
        _description.gameObject.SetActive(true);
        _imageMobilePlatform.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        _description.gameObject.SetActive(false);
        _imageMobilePlatform.gameObject.SetActive(false);
    }
}
