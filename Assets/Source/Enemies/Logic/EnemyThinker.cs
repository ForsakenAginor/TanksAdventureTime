using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyThinker
    {
        private const float MinDelay = 0f;
        private const float MaxDelay = 0.5f;

        private readonly CancellationTokenSource _cancellation;
        private readonly TimeSpan _time;

        private bool _isBusy;

        public EnemyThinker(float delay)
        {
            _cancellation = new CancellationTokenSource();
            _time = TimeSpan.FromSeconds(delay + Random.Range(MinDelay, MaxDelay));
        }

        public event Action Updated;

        public void Start()
        {
            _isBusy = true;
            Update().Forget();
        }

        public void Stop()
        {
            _isBusy = false;

            if (_cancellation.IsCancellationRequested == true)
                return;

            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid Update()
        {
            while (_isBusy == true)
            {
                await UniTask.Delay(_time, cancellationToken: _cancellation.Token);
                Updated?.Invoke();
            }
        }
    }
}