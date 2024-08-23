using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DestructionObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class Destruction : MonoBehaviour, IPermanentKiller, IReactive, ISupportStructure
    {
        private const float FallOffset = 0.5f;

        [SerializeField] private Transform _panelDestruction;
        [SerializeField] private ParticleSystem _particleSystem;

        private Transform[] _transformObjects;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private CancellationTokenSource _cancellation;
        private float _startY;

        public event Action Waked;
        public event Action<List<MeshRenderer>> Destroyed;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.Sleep();
            _cancellation = new CancellationTokenSource();
            _startY = _transform.position.y;
            Init();

            if (Waked != null)
                WaitWaking().Forget();
        }

        private void OnDestroy()
        {
            StopWaiting();
        }

        public void React()
        {
            _panelDestruction.position = _transform.position;
            _panelDestruction.rotation = _transform.rotation;
            _panelDestruction.gameObject.SetActive(true);
            gameObject.SetActive(false);

            if (Waked != null)
            {
                StopWaiting();
                Waked.Invoke();
            }

            Destroyed?.Invoke(_panelDestruction.GetComponentsInChildren<MeshRenderer>().ToList());
        }

        private void Init()
        {
            _transformObjects = new Transform[_panelDestruction.childCount];

            for (int i = 0; i < _transformObjects.Length; i++)
            {
                _transformObjects[i] = _panelDestruction.GetChild(i);
                _transformObjects[i].gameObject.AddComponent<DestroyedPart>();
            }
        }

        private async UniTaskVoid WaitWaking()
        {
            await UniTask.WaitUntil(CanWakeUp, cancellationToken: _cancellation.Token);
            React();
        }

        private void StopWaiting()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
            _cancellation = null;
        }

        private bool CanWakeUp()
        {
            return _startY - _transform.position.y > FallOffset;
        }
    }
}