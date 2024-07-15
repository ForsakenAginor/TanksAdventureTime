using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class PointsSpotsCollection : MonoBehaviour
    {
        [SerializeField] private Transform[] _smallBuildingSpots;
        [SerializeField] private Transform[] _mediumBuildingSpots;
        [SerializeField] private Transform[] _largeBuildingSpots;

        public IEnumerable<Vector3> SmallBuildingSpots => _smallBuildingSpots.Select(o => o.position);
        public IEnumerable<Vector3> MediumBuildingSpots => _mediumBuildingSpots.Select(o => o.position);
        public IEnumerable<Vector3> LargeBuildingSpots => _largeBuildingSpots.Select(o => o.position);
    }
}