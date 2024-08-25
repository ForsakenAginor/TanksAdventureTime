using System;
using UnityEngine;

namespace Enemies
{
    public class CollisionActivator : IDisposable
    {
        private readonly GameObject _gameObject;
        private readonly Collider _collider;
        private readonly ISupportStructure _structure;

        public CollisionActivator(GameObject gameObject, Collider collider, ISupportStructure structure)
        {
            _gameObject = gameObject;
            _collider = collider;
            _structure = structure;
            _collider.enabled = false;

            _structure.Waked += OnSupportDestroyed;
            _structure.StartWaking();
        }

        public void Dispose()
        {
            _structure.Waked -= OnSupportDestroyed;
        }

        private void OnSupportDestroyed()
        {
            _collider.enabled = true;
            _gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}