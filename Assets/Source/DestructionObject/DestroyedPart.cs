using UnityEngine;

namespace DestructionObject
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class DestroyedPart : MonoBehaviour, IPermanentKiller, IReactive
    {
        private const string DieObject = nameof(DisableObject);

        private Rigidbody _body;
        private Collider _collider;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void React()
        {
            float minValue = 5f;
            float maxValue = 10f;
            float timeDie = Random.Range(minValue, maxValue);
            Invoke(DieObject, timeDie);
        }

        private void DisableObject()
        {
            _body.velocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _body.useGravity = false;
            _collider.enabled = false;
            _body.detectCollisions = false;
        }
    }
}