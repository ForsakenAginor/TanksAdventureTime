using UnityEngine;

namespace Assets.Source.Player.HealthSystem
{
    [RequireComponent(typeof(Collider))]
    public class PlayerAsTarget : MonoBehaviour, IPlayerTarget, IPermanentKiller
    {
        private Transform _transform;
        private Collider _collider;

        public Vector3 Position => _transform.position;

        private void Awake()
        {
            _transform = transform;
            _collider = GetComponent<Collider>();
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }

        public void TakeHit(HitTypes type)
        {
            if (type != HitTypes.Explosion)
                return;
        }
    }
}