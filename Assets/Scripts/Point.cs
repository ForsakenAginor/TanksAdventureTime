using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private List<EnemySetup> _enemies;
    [SerializeField] private Transform _player;
    [SerializeField] private float _minDistance;
    [SerializeField] private RectTransform _marker;
    [SerializeField] private float _markerSize = 25;

    private void FixedUpdate()
    {
        Transform closest = _enemies.
                        Select(o => o.transform).
                        OrderBy(o => (o.position - _player.position).sqrMagnitude).
                        First();

        float distance = (closest.position - _player.position).magnitude;

        if(distance > _minDistance)
        {
            if(_marker.gameObject.activeSelf == false)
                _marker.gameObject.SetActive(true);

            DoThing(closest.position);
        }
        else
        {
            if (_marker.gameObject.activeSelf)
                _marker.gameObject.SetActive(false);
        }
    }

    private void DoThing(Vector3 enemyPosition)
    {
        Vector2 yAxis = new Vector2(_player.forward.x, _player.forward.z);
        Vector3 tempVector = enemyPosition - _player.position;
        Vector2 enemySpot = new Vector2(tempVector.x, tempVector.z);
        float cos = Vector2.Dot(enemySpot, yAxis)/enemySpot.magnitude / yAxis.magnitude;
        Vector3 cross = Vector3.Cross(enemySpot, yAxis);
        float sin = cross.magnitude / enemySpot.magnitude / yAxis.magnitude;

        sin = cross.z < 0 ? -sin : sin;

        float screenWidth = Screen.width / 2;
        float screemHeight = Screen.height / 2;
        float width = screenWidth * sin;
        float height = screemHeight * cos;
        width = Mathf.Clamp(width, -screenWidth + _markerSize, screenWidth -  _markerSize);
        height = Mathf.Clamp(height, -screemHeight + _markerSize, screemHeight -  _markerSize);

        _marker.localPosition = new Vector3(width, height, 0);
    }
}
