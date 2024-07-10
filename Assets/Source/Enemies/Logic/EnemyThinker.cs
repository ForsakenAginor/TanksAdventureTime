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

        private readonly CancellationTokenSource Cancellation;
        private readonly TimeSpan Time;

        private bool _isBusy;

        public EnemyThinker(float delay)
        {
            Cancellation = new CancellationTokenSource();
            Time = TimeSpan.FromSeconds(delay + Random.Range(MinDelay, MaxDelay));
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

            if (Cancellation.IsCancellationRequested == true)
                return;

            Cancellation.Cancel();
            Cancellation.Dispose();
        }

        private async UniTaskVoid Update()
        {
            while (_isBusy == true)
            {
                await UniTask.Delay(Time, cancellationToken: Cancellation.Token);
                Updated?.Invoke();
            }
        }
    }
}