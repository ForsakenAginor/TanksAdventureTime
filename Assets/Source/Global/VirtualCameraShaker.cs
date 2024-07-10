using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VirtualCameraShaker
{
    private const float MinValue = (float)ValueConstants.Zero;

    private readonly float ShakeDuration;
    private readonly float ShakeAmplitude;

    private Collider _collider;
    private float _timer = 0f;
    private readonly CinemachineBasicMultiChannelPerlin CameraNoise;
    private readonly CancellationToken Token;

    public VirtualCameraShaker(
        CinemachineVirtualCamera virtualCamera,
        CancellationToken destroyCancellationToken,
        float shakeDuration = 0.8f,
        float shakeAmplitude = 5f,
        float shakeFrequency = 7f)
    {
        CameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Token = destroyCancellationToken;
        ShakeDuration = shakeDuration;
        ShakeAmplitude = shakeAmplitude;
        CameraNoise.m_FrequencyGain = shakeFrequency;
    }

    public void Shake()
    {
        StartShaking().Forget();
    }

    private async UniTaskVoid StartShaking()
    {
        _timer = ShakeDuration;

        while (_timer > MinValue)
        {
            CameraNoise.m_AmplitudeGain = ShakeAmplitude;
            _timer -= Time.deltaTime;
            await UniTask.NextFrame(Token);
        }

        CameraNoise.m_AmplitudeGain = MinValue;
        _timer = MinValue;
    }
}