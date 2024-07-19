using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SpawnerPointsWithEnemies
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _prefabsEnemy;
        [SerializeField] private int _needCountForSpawn;

        private Point[] _points;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            InitPoints();
            Spawn();
        }

        private void InitPoints()
        {
            _points = new Point[_transform.childCount];

            for (int i = 0; i < _points.Length; i++)
                _points[i] = _transform.GetChild(i).AddComponent<Point>();
        }

        private void Spawn()
        {
            List<int> freePlace;
            int minValue = 0;
            int negativeValue = -1;

            if (_needCountForSpawn < minValue)
                _needCountForSpawn *= negativeValue;

            if (_needCountForSpawn > _points.Length)
                _needCountForSpawn = _points.Length;

            for (int i = 0; i < _needCountForSpawn; i++)
            {
                freePlace = GetListFreePlace();
                int randomPoint = freePlace[Random.Range(minValue, freePlace.Count)];
                Instantiate(_prefabsEnemy[Random.Range(minValue, _prefabsEnemy.Count)],
                _points[randomPoint].transform);
                _points[randomPoint].TakeThePlace();
            }
        }

        private List<int> GetListFreePlace()
        {
            List<int> freePointIndexes = new List<int>();

            for (int i = 0; i < _points.Length; i++)
            {
                if (_points[i].IsFreePlace == false)
                    freePointIndexes.Add(i);
            }

            return freePointIndexes;
        }
    }
}