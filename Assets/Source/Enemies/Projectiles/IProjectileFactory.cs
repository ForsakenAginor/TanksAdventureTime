using UnityEngine;

namespace Enemies
{
    public interface IProjectileFactory
    {
        public void Create(Vector3 position, Vector3 targetPosition, Vector3 direction, Vector3 forward);
    }
}