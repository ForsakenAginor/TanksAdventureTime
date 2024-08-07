using Cinemachine;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider))]
    public class TargetTest : MonoBehaviour, IPlayerTarget, IPermanentKiller // Используется только в EnemyTesting сцене
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _shakeDuration = 1f;
        [SerializeField] private float _shakeAmplitude = 1f;
        [SerializeField] private float _shakeFrequency = 1f;
        [SerializeField] private Transform _viewPoint;

        private Collider _collider;
        private VirtualCameraShaker _shaker;

        public Vector3 Position => _viewPoint.position;

        public TargetPriority Priority => TargetPriority.Medium;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _shaker = new VirtualCameraShaker(
                _camera,
                destroyCancellationToken,
                _shakeDuration,
                _shakeAmplitude,
                _shakeFrequency);
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }

        public void TakeHit(HitTypes type)
        {
            if (type != HitTypes.Explosion)
                return;

            _shaker.Shake();
        }
    }
}