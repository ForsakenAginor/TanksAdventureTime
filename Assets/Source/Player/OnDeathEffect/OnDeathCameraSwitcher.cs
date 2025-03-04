using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    public class OnDeathCameraSwitcher : ICancelableOnDeathEffect
    {
        private const float RaycastLength = 5f;

        private readonly CinemachineVirtualCamera _followCamera;
        private readonly CinemachineVirtualCamera _deathCamera;
        private readonly float _blendTime;

        private List<MeshRenderer> _meshRenderers;

        public OnDeathCameraSwitcher(
            CinemachineVirtualCamera followCamera,
            CinemachineVirtualCamera deathCamera,
            float blendTime)
        {
            _followCamera = followCamera != null ? followCamera : throw new ArgumentNullException(nameof(followCamera));
            _deathCamera = deathCamera != null ? deathCamera : throw new ArgumentNullException(nameof(deathCamera));
            _blendTime = blendTime >= 0 ? blendTime : throw new ArgumentOutOfRangeException(nameof(blendTime));
        }

        public void ReturnToNormalState()
        {
            _deathCamera.Priority = _followCamera.Priority - 1;

            if (_meshRenderers is { Count: > 0 })
                _meshRenderers.ForEach(o => o.enabled = true);
        }

        public async void Switch()
        {
            _deathCamera.Priority = _followCamera.Priority + 1;
            Vector3 direction = _deathCamera.Follow.transform.position - _deathCamera.transform.position;

            if (Physics.Raycast(_deathCamera.transform.position, direction, out RaycastHit rayCastInfo, RaycastLength))
            {
                int delay = (int)(_blendTime * 2000) / 3;
                await UniTask.Delay(delay);
                _meshRenderers = rayCastInfo.collider.GetComponentsInChildren<MeshRenderer>().ToList();
                _meshRenderers.ForEach(o => o.enabled = false);
            }
        }
    }
}