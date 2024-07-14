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

            _structure.Destroyed += OnSupportDestroyed;
        }

        public void Dispose()
        {
            _structure.Destroyed -= OnSupportDestroyed;
        }

        private void OnSupportDestroyed()
        {
            _collider.enabled = true;
            _gameObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}