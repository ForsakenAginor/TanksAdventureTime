using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DestructionObject
{
    public class BunkerHealthView : MonoBehaviour
    {
        private const int WaitSeconds = 3;

        [SerializeField] private Slider _slider;
        [SerializeField] private Bunker _bunker;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds;

        private void Awake()
        {
            _slider.value = _slider.maxValue;
            _waitForSeconds = new WaitForSeconds(WaitSeconds);
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

        private void EnableSlider()
        {
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
}