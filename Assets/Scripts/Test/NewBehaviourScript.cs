using Assets.Source.Difficulty;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Button _button;
    private LevelData _levelData;

    private void Start()
    {
        _levelData = new LevelData();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(AbortProgress);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(AbortProgress);
    }

    private void AbortProgress()
    {
        _levelData.SaveLevel(0);
    }
}
