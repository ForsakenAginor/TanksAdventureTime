using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private MovingSystem _movingSystem;
    [SerializeField] private AimSystem _aimSystem;
    [SerializeField] private FireSystem _fireSystem;

    private void Awake()
    {
        PlayerInput playerInput = new ();
        _movingSystem.Init(playerInput);
        _aimSystem.Init(playerInput);
        _fireSystem.Init(playerInput);
    }
}
