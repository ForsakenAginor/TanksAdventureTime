using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DestructionObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class Destruction : MonoBehaviour, IPermanentKiller, IReactive, ISupportStructure
    {
        [SerializeField] private Transform _panelDestruction;
        [SerializeField] private ParticleSystem _particleSystem;

        private Transform[] _transformObjects;
        private Transform _transform;
        private Rigidbody _rigidbody;

        public event Action Destroyed;

        private void Start()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.Sleep();
            Init();
        }

        public void React()
        {
            _panelDestruction.position = _transform.position;
            _panelDestruction.rotation = _transform.rotation;
            _panelDestruction.gameObject.SetActive(true);
            gameObject.SetActive(false);
            Destroyed?.Invoke();
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