using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Characters
{
    public class CharacterThinker
    {
        private const float MinDelay = 0f;
        private const float MaxDelay = 0.5f;

        private readonly TimeSpan _time;

        private CancellationTokenSource _cancellation;

        public CharacterThinker(float delay)
        {
            _time = TimeSpan.FromSeconds(delay + Random.Range(MinDelay, MaxDelay));
        }

        public event Action Updated;

        public void Start()
        {
            _cancellation = new CancellationTokenSource();
            Update().Forget();
        }

        public void Stop()
        {
            if (_cancellation == null)
                return;

            if (_cancellation.IsCancellationRequested == true)
                return;

            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid Update()
        {
            while (_cancellation.IsCancellationRequested == false)
            {
                await UniTask.Delay(_time, cancellationToken: _cancellation.Token);
                Updated?.Invoke();
            }
        }
    }
}