﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class PointsSpotsCollection : MonoBehaviour
    {
        [SerializeField] private Transform[] _smallBuildingSpots;
        [SerializeField] private Transform[] _mediumBuildingSpots;
        [SerializeField] private Transform[] _largeBuildingSpots;
        [SerializeField] private Transform[] _obstaclesSpots;

        public IEnumerable<Transform> SmallBuildingSpots => _smallBuildingSpots;

        public IEnumerable<Transform> MediumBuildingSpots => _mediumBuildingSpots;

        public IEnumerable<Transform> LargeBuildingSpots => _largeBuildingSpots;

        public IEnumerable<Transform> ObstaclesSpots => _obstaclesSpots;
    }
}