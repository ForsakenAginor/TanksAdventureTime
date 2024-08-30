using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VirtualCameraShaker
{
    private const float MinValue = (float)ValueConstants.Zero;

    private readonly float _shakeDuration;
    private readonly float _shakeAmplitude;
    private readonly CinemachineBasicMultiChannelPerlin _cameraNoise;
    private readonly CancellationToken _token;

    private float _timer;

    public VirtualCameraShaker(
        CinemachineVirtualCamera virtualCamera,
        CancellationToken destroyCancellationToken,
        float shakeDuration = 0.8f,
        float shakeAmplitude = 5f,
        float shakeFrequency = 7f)
    {
        _cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _token = destroyCancellationToken;
        _shakeDuration = shakeDuration;
        _shakeAmplitude = shakeAmplitude;
        _cameraNoise.m_FrequencyGain = shakeFrequency;
    }

    public void Shake()
    {
        StartShaking().Forget();
    }

    private async UniTaskVoid StartShaking()
    {
        _timer = _shakeDuration;

        while (_timer > MinValue)
        {
            _cameraNoise.m_AmplitudeGain = _shakeAmplitude;
            _timer -= Time.deltaTime;
            await UniTask.NextFrame(_token);
        }

        _cameraNoise.m_AmplitudeGain = MinValue;
        _timer = MinValue;
    }
}