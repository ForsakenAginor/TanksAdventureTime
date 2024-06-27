using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Enemies
{
    public class EnemyThinker
    {
        private readonly CancellationToken Token;
        private readonly TimeSpan Time;

        public EnemyThinker(CancellationToken token, float delay)
        {
            Token = token;
            Time = TimeSpan.FromSeconds(delay);
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