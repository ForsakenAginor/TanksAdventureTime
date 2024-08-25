using UnityEngine;

public class StartTraining : MonoBehaviour
{
    [SerializeField] private TrainingMobilePlatform _trainingMobilePlatform;
    [SerializeField] private SaveService _saveService;

    private void OnEnable() => _saveService.Loaded += OnStartTraining;

    private void OnDisable() => _saveService.Loaded -= OnStartTraining;

    private void OnStartTraining()
    {
        int minValue = 0;
        int maxValue = 1;
        bool isTraining = true ? _saveService.CompletedTrainingOnMobile == maxValue :
                                 _saveService.CompletedTrainingOnMobile == minValue;

        if (isTraining == true) return;

        if (Application.isMobilePlatform)
            _trainingMobilePlatform.gameObject.SetActive(true);
    }
}