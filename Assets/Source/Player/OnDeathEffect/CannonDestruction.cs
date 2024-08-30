using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class CannonDestruction : ICancelableOnDeathEffect
    {
        private const float Half = 0.5f;
        private const int Loops = 2;
        private const float HeightOffset = 1.5f;

        private readonly Transform _cannon;
        private readonly float _effectSpeed;
        private readonly float _distance;
        private readonly float _height;
        private readonly Vector3 _startPosition;
        private readonly Vector3 _targetRotation = new (0, 0, 180);

        public CannonDestruction(Transform cannon, float effectSpeed, float distance, float height)
        {
            _cannon = cannon != null ? cannon : throw new ArgumentNullException(nameof(cannon));
            _effectSpeed = effectSpeed > 0 ? effectSpeed : throw new ArgumentOutOfRangeException(nameof(effectSpeed));
            _distance = distance;
            _height = height;
            _startPosition = cannon.position;
        }

        public void ReturnToNormalState()
        {
            _cannon.position = _startPosition;
            _cannon.rotation = Quaternion.identity;
        }

        public void Detonate()
        {
            Vector3 position = _cannon.position;
            float xPosition = position.x + _distance;
            float zPosition = position.z + (_distance * Half);
            float yTopPosition = position.y + _height;
            float yBottomPosition = position.y - HeightOffset;
            _cannon.DOMoveX(xPosition, _effectSpeed).SetEase(Ease.Linear);
            _cannon.DOMoveZ(zPosition, _effectSpeed).SetEase(Ease.Linear);
            Sequence ySequence = DOTween.Sequence();
            ySequence.Append(_cannon.DOMoveY(yTopPosition, _effectSpeed * Half).SetEase(Ease.Linear));
            ySequence.Append(_cannon.DOMoveY(yBottomPosition, _effectSpeed * Half).SetEase(Ease.Linear));
            _cannon.DORotate(_targetRotation, _effectSpeed * Half).SetEase(Ease.Linear)
                .SetLoops(Loops, LoopType.Incremental);
        }
    }
}