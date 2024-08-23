using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyCollision : MonoBehaviour, IDamageableTarget, IReactive
    {
        private Transform _viewPoint;
        private ISupportStructure _structure;

        public event Action<HitTypes> HitTook;

        public Vector3 Position => _viewPoint.position;

        public TargetPriority Priority { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IPermanentKiller killer) == false)
                return;

            if (killer is ISupportStructure && killer == _structure)
                return;

            TakeHit(HitTypes.PermanentDeath);
        }

        public void Init(Transform viewPoint, TargetPriority priority, ISupportStructure structure)
        {
            _viewPoint = viewPoint;
            _structure = structure;
            Priority = priority;
        }

        public void TakeHit(HitTypes type)
        {
            HitTook?.Invoke(type);
        }

        public void React()
        {
            TakeHit(HitTypes.PlayerExplosion);
        }

        public void SetPriority(TargetPriority priority)
        {
            Priority = priority;
        }
    }
}