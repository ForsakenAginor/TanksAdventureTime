using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private RectTransform _screen;

        private readonly float _imageSize = 25;
        private IEnumerable<ITarget> _enemies;
        private Transform _player;
        private float _minDistance;
        private RectTransform _image;

        private void FixedUpdate()
        {
            if (_enemies == null || _enemies.Count() == 0)
                return;

            Vector3 closest = _enemies.
                            Select(o => o.Position).
                            OrderBy(o => (o - _player.position).sqrMagnitude).
                            First();

            float distance = (closest - _player.position).magnitude;

            if (distance > _minDistance)
            {
                if (_image.gameObject.activeSelf == false)
                    _image.gameObject.SetActive(true);

                MoveMarkerImage(closest);
            }
            else
            {
                if (_image.gameObject.activeSelf)
                    _image.gameObject.SetActive(false);
            }
        }

        public void Init(IEnumerable<ITarget> enemies, Transform player, float minDistance, RectTransform marker)
        {
            _enemies = enemies != null ? enemies : throw new ArgumentNullException(nameof(enemies));
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));
            _image = marker != null ? marker : throw new ArgumentNullException(nameof(marker));
            _minDistance = minDistance > 0 ? minDistance : throw new ArgumentNullException(nameof(minDistance));
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

            float screenWidth = _screen.rect.width / 2;
            float screemHeight = _screen.rect.height / 2;
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