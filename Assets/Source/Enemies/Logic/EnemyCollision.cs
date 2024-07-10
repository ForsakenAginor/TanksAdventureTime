using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyCollision : MonoBehaviour, IDamageableTarget, IReactive
    {
        private Transform _transform;

        public event Action<HitTypes> HitTook;

        public Vector3 Position => _transform.position;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IPermanentKiller _) == false)
                return;

            TakeHit(HitTypes.PermanentDeath);
        }

        public void Init(Transform transform)
        {
            _transform = transform;
        }

        public void TakeHit(HitTypes type)
        {
            HitTook?.Invoke(type);
        }

        public void React()
        {
            TakeHit(HitTypes.Explosion);
        }
    }
}