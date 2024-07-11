using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters
{
    public class CharacterRotator
    {
        private readonly float _rotationSpeed;
        private readonly Transform _transform;
        private readonly ITarget _target;

        private CancellationTokenSource _cancellationSource;

        public CharacterRotator(float rotationSpeed, Transform transform, ITarget target)
        {
            _rotationSpeed = rotationSpeed;
            _transform = transform;
            _target = target;
        }

        public void StartRotation()
        {
            _cancellationSource = new CancellationTokenSource();
            Rotate().Forget();
        }

        public void StopRotation()
        {
            if (_cancellationSource.IsCancellationRequested == true)
                return;

            _cancellationSource.Cancel();
            _cancellationSource.Dispose();
        }

        private async UniTaskVoid Rotate()
        {
            while (_cancellationSource.IsCancellationRequested == false)
            {
                Vector3 direction = _target.Position - _transform.position;
                Quaternion look =
                    Quaternion.LookRotation(new Vector3(direction.x, (float)ValueConstants.Zero, direction.z));
                _transform.rotation = Quaternion.RotateTowards(_transform.rotation, look, _rotationSpeed);
                await UniTask.NextFrame(_cancellationSource.Token);
            }
        }
    }
}