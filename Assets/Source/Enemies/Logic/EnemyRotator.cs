using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    public class EnemyRotator
    {
        private readonly float RotationSpeed;
        private readonly Transform Transform;
        private readonly ITarget Target;

        private CancellationTokenSource _cancellationSource;

        public EnemyRotator(float rotationSpeed, Transform transform, ITarget target)
        {
            RotationSpeed = rotationSpeed;
            Transform = transform;
            Target = target;
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
                Vector3 direction = Target.Position - Transform.position;
                Quaternion look =
                    Quaternion.LookRotation(new Vector3(direction.x, (float)ValueConstants.Zero, direction.z));
                Transform.rotation = Quaternion.RotateTowards(Transform.rotation, look, RotationSpeed);
                await UniTask.NextFrame(_cancellationSource.Token);
            }
        }
    }
}