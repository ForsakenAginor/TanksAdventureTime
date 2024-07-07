using Agava.WebUtility;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _losingPanel;
    [SerializeField] private GameObject _winingPanel;
    [SerializeField] private GameObject _mobileInputCanvas;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private float _delay;

    private void Start()
    {
        /* 
        uncomment that on publishing

        if(Device.IsMobile == false)
            _mobileInputCanvas.SetActive(false);
        */
    }

    public void ShowLosingPanel()
    {
        _buttonsPanel.SetActive(false);
        _mobileInputCanvas.SetActive(false);
        StartCoroutine(DisplayLosingPanel());
    }

    public void ShowWiningPanel()
    {
        _buttonsPanel.SetActive(false);
        _mobileInputCanvas.SetActive(false);
        _winingPanel.SetActive(true);
    }

    private IEnumerator DisplayLosingPanel()
    {
        WaitForSeconds delay = new (_delay);
        yield return delay;
        _losingPanel.SetActive(true);
    }
}
