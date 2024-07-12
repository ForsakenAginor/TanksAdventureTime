using Enemies;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Marker
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private List<EnemySetup> _enemies;
        [SerializeField] private Transform _player;
        [SerializeField] private float _minDistance;
        [SerializeField] private RectTransform _image;
        [SerializeField] private float _imageSize = 25;

        private void FixedUpdate()
        {
            if (_enemies.Count == 0)
                return;

            Transform closest = _enemies.
                            Select(o => o.transform).
                            OrderBy(o => (o.position - _player.position).sqrMagnitude).
                            First();

            float distance = (closest.position - _player.position).magnitude;

            if (distance > _minDistance)
            {
                if (_image.gameObject.activeSelf == false)
                    _image.gameObject.SetActive(true);

                MoveMarkerImage(closest.position);
            }
            else
            {
                if (_image.gameObject.activeSelf)
                    _image.gameObject.SetActive(false);
            }
        }

        private void MoveMarkerImage(Vector3 enemyPosition)
        {
            Vector2 yAxis = new Vector2(_player.forward.x, _player.forward.z);
            Vector3 tempVector = enemyPosition - _player.position;
            Vector2 enemySpot = new Vector2(tempVector.x, tempVector.z);
            float cos = Vector2.Dot(enemySpot, yAxis) / enemySpot.magnitude / yAxis.magnitude;
            Vector3 cross = Vector3.Cross(enemySpot, yAxis);
            float sin = cross.magnitude / enemySpot.magnitude / yAxis.magnitude;

            sin = cross.z < 0 ? -sin : sin;

            float screenWidth = Screen.width / 2;
            float screemHeight = Screen.height / 2;
            float width = screenWidth * sin;
            float height = screemHeight * cos;
            float maxWidth = screenWidth - _imageSize;
            float maxHeight = screemHeight - _imageSize;
            width = Mathf.Clamp(width, -maxWidth, maxWidth);
            height = Mathf.Clamp(height, -maxHeight, maxHeight);
            float multiplier = 1f;

            if (Mathf.Abs(cos) >= Mathf.Abs(sin) && Mathf.Abs(height) < maxHeight)
                multiplier = maxHeight / Mathf.Abs(height);
            else if (Mathf.Abs(cos) < Mathf.Abs(sin) && Mathf.Abs(width) < maxWidth)
                multiplier = maxWidth / Mathf.Abs(width);

            width *= multiplier;
            height *= multiplier;

            _image.localPosition = new Vector3(width, height, 0);
            float zRotation = Mathf.Rad2Deg * Mathf.Acos(cos);
            zRotation = sin < 0 ? zRotation : -zRotation;
            _image.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }
}