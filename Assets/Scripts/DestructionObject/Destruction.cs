using Unity.VisualScripting;
using UnityEngine;

namespace DestructionObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class Destruction : MonoBehaviour, IPermanentKiller
    {
        [SerializeField] private Transform _panelDestruction;
        [SerializeField] private ParticleSystem _particleSystem;

        private Transform[] _transformObjects;
        private Transform _transform;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.Sleep();
            Init();
            _transform = transform;
        }

        public void DestroyObject()
        {
            _panelDestruction.transform.position = _transform.position;
            _panelDestruction.rotation = _transform.rotation;
            _panelDestruction.gameObject.SetActive(true);
            _transform.gameObject.SetActive(false);
        }

        private void Init()
        {
            _transformObjects = new Transform[_panelDestruction.childCount];

            for (int i = 0; i < _transformObjects.Length; i++)
            {
                _transformObjects[i] = _panelDestruction.GetChild(i);
                _transformObjects[i].AddComponent<DestroyedPart>();
            }
        }
    }
}