using UnityEngine;
using UnityEngine.UI;

public class LearnTraningGame : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private SaveService _saveService;
    [SerializeField] private TraningMoveTank _traningMoveTank;


    private void OnEnable()
    {
        _playButton.onClick.AddListener(EnableTraining);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(EnableTraining);
    }

    private void EnableTraining()
    {
        _traningMoveTank.TurnOn(Application.isMobilePlatform);
    }
}
