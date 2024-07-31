using UnityEngine;
using UnityEngine.UI;

public class TestButtonRandom : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private SaveService _saveService;

    private void OnEnable()
    {
        _button.onClick.AddListener(Save);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Save);
    }

    private void Save()
    {
        _saveService.SaveLevel(Random.Range(1, 6));
        _saveService.SavePlayerHelper(Random.Range(1, 6));
        _saveService.SaveCompletedTraining(true);
    }
}
