using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DestructionObject;

public class BukcerHealthView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Bunker _bunker;
    [SerializeField] private Camera _camera;

    private Transform _transform;
    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds;
    private float _waitSeconds = 3;

    private void Awake()
    {
        _transform = transform;
        _slider.value = _slider.maxValue;
        _waitForSeconds = new WaitForSeconds(_waitSeconds);
        _slider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _bunker.TookDamage += OnChangeValue;
        _bunker.Died += EnableSlider;
    }

    private void OnDisable()
    {
        _bunker.TookDamage -= OnChangeValue;
        _bunker.Died -= EnableSlider;
    }

    private void Update() => _transform.rotation = _camera.transform.rotation;

    private void EnableSlider(Action died)
    {
        died?.Invoke();
        _slider.gameObject.SetActive(false);
    }

    private void OnChangeValue(float currentValue)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(currentValue));
    }

    private IEnumerator ChangeValue(float currentValue)
    {
        _slider.gameObject.SetActive(true);
        _slider.value = currentValue;
        yield return _waitForSeconds;
        _slider.gameObject.SetActive(false);
        StopCoroutine(_coroutine);
    }
}