using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    public class PointsSpotsCollection : MonoBehaviour
    {
        [SerializeField] private Transform[] _smallBuildingSpots;
        [SerializeField] private Transform[] _mediumBuildingSpots;
        [SerializeField] private Transform[] _largeBuildingSpots;
        [SerializeField] private Transform[] _obstaclesSpots;
        [SerializeField] private Transform[] _bunkerSpots;

        public IEnumerable<Transform> SmallBuildingSpots => _smallBuildingSpots;

        public IEnumerable<Transform> MediumBuildingSpots => _mediumBuildingSpots;

        public IEnumerable<Transform> LargeBuildingSpots => _largeBuildingSpots;

        public IEnumerable<Transform> ObstaclesSpots => _obstaclesSpots;

        public IEnumerable<Transform> BunkerSpots => _bunkerSpots;
    }
}