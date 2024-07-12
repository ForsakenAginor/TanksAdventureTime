using UnityEngine;

namespace SpawnerPointsWithEnemies
{
    public class Point : MonoBehaviour
    {
        public bool IsFreePlace { get; private set; } = false;

        public void TakeThePlace() => IsFreePlace = true;
    }
}