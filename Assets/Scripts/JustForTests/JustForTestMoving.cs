using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JustForTestMoving : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 direction = transform.forward.normalized;
        _rigidbody.velocity = (direction * _speed * Time.deltaTime * Input.GetAxis(Vertical));

        transform.Rotate(Vector3.up, Input.GetAxis(Horizontal) * Time.deltaTime * _rotationSpeed);
    }
}
