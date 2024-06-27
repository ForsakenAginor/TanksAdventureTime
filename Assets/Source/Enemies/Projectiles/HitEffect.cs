using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    public class HitEffect : SpawnableObject
    {
        [SerializeField] private ParticleSystem _particle;

        public void Init(Vector3 position)
        {
            Transform.rotation = Quaternion.LookRotation(position - Transform.position);
            OnPlayed().Forget();
        }

        private async UniTaskVoid OnPlayed()
        {
            _particle.Play();

            while (_particle.isPlaying == true)
                await UniTask.NextFrame(destroyCancellationToken);

            Push();
        }
    }
}