using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingSystem : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = new ();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        Vector2 input = _playerInput.ReadMovement();
        Vector3 movingDirection = Vector3.zero;

        if (Mathf.Approximately(input.y, 0f) == false )
            movingDirection = (transform.forward * input.y).normalized;

        _rigidbody.velocity = (movingDirection * _speed * Time.deltaTime );
        transform.Rotate(Vector3.up, input.x * Time.deltaTime * _rotationSpeed);
    }
}
