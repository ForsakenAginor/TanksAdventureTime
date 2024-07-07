using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider))]
    public class TargetTest : MonoBehaviour, IPlayerTarget, IPermanentKiller
    {
        private const float MinValue = (float)ValueConstants.Zero;

        [SerializeField] private Transform _transform;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _shakeDuration = 1f;
        [SerializeField] private float _shakeAmplitude = 1f;
        [SerializeField] private float _shakeFrequency = 1f;

        private Collider _collider;
        private float _timer = 0f;
        private CinemachineBasicMultiChannelPerlin _cameraNoise;

        public Vector3 Position => _transform.position;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _cameraNoise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _cameraNoise.m_FrequencyGain = _shakeFrequency;
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }

        public void TakeHit(HitTypes type)
        {
            if (type != HitTypes.Explosion)
                return;

            Shake().Forget();
        }

        private async UniTaskVoid Shake()
        {
            _timer = _shakeDuration;

            while (_timer > MinValue)
            {
                _cameraNoise.m_AmplitudeGain = _shakeAmplitude;
                _timer -= Time.deltaTime;
                await UniTask.NextFrame(destroyCancellationToken);
            }

            _cameraNoise.m_AmplitudeGain = MinValue;
            _timer = MinValue;
        }
    }
}