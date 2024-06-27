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

        private readonly CancellationToken Token;
        private readonly TimeSpan Time;

        public EnemyThinker(CancellationToken token, float delay)
        {
            Token = token;
            Time = TimeSpan.FromSeconds(delay + Random.Range(MinDelay, MaxDelay));
        }

        public event Action Updated;

        public void Start()
        {
            Update().Forget();
        }

        private async UniTaskVoid Update()
        {
            while (Token.IsCancellationRequested == false)
            {
                await UniTask.Delay(Time, cancellationToken: Token);
                Updated?.Invoke();
            }
        }
    }
}