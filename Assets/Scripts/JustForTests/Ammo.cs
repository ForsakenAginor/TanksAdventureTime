using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ammo : MonoBehaviour
{
    [SerializeField] private float _lifetime = 5f;

    private Rigidbody _body;
    private AmmoPool _pull;
    private Coroutine _fly;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;
        _pull.Push(this);
        StopCoroutine(_fly);
    }

    public void Init(AmmoPool pull)
    {
        _pull = pull != null ? pull : throw new ArgumentNullException(nameof(pull));
        _fly = StartCoroutine(Flying());
    }

    public void Launch(Vector3 position, Vector3 velocity)
    {
        transform.position = position;
        _body.velocity = velocity;
    }

    private IEnumerator Flying()
    {
        WaitForSeconds delay = new WaitForSeconds(_lifetime);

        yield return delay;
        _pull.Push(this);
    }
}
