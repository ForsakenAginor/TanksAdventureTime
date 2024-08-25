using UnityEngine;

public class TrainingComputerPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _backGround;

    private void Awake()
    {
        _backGround.SetActive(false);
    }
}
