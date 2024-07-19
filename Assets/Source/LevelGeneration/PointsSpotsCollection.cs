﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class PointsSpotsCollection : MonoBehaviour
    {
        [SerializeField] private Transform[] _smallBuildingSpots;
        [SerializeField] private Transform[] _mediumBuildingSpots;
        [SerializeField] private Transform[] _largeBuildingSpots;

        public IEnumerable<Transform> SmallBuildingSpots => _smallBuildingSpots;
        public IEnumerable<Transform> MediumBuildingSpots => _mediumBuildingSpots;
        public IEnumerable<Transform> LargeBuildingSpots => _largeBuildingSpots;
    }
}