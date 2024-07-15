using Agava.WebUtility;
using Assets.Source.Marker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Marker))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _losingPanel;
    [SerializeField] private GameObject _winingPanel;
    [SerializeField] private GameObject _mobileInputCanvas;
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private float _delay;

    [Header("Marker")]
    [SerializeField] private float _minDistance;
    [SerializeField] private RectTransform _markerImage;
    private Marker _marker;

    private void Awake()
    {
        _marker = GetComponent<Marker>();
    }

    private void Start()
    {
        /* 
        uncomment that on publishing

        if(Device.IsMobile == false)
            _mobileInputCanvas.SetActive(false);
        */
    }

    public void Init(IEnumerable<Transform> enemies, Transform player)
    {
        if (enemies == null)
            throw new ArgumentNullException(nameof(enemies));

        if (player == null)
            throw new ArgumentNullException(nameof(player));

        _marker.Init(enemies, player, _minDistance, _markerImage);
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
