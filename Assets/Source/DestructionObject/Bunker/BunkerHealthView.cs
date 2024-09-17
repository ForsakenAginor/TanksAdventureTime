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
            _bunker.DamageTaking += OnDamageTaking;
            _bunker.Died += OnDied;
        }

        private void OnDisable()
        {
            _bunker.DamageTaking -= OnDamageTaking;
            _bunker.Died -= OnDied;
        }

        private void OnDied()
        {
            _slider.gameObject.SetActive(false);
        }

        private void OnDamageTaking(float currentValue)
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