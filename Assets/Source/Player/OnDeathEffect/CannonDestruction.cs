using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class CannonDestruction : ICancelableOnDeathEffect
    {
        private readonly Transform _cannon;
        private readonly float _effectSpeed;
        private readonly float _distance;
        private readonly float _height;
        private readonly Vector3 _startPosition;

        public CannonDestruction(Transform cannon, float effectSpeed, float distance, float heigth)
        {
            _cannon = cannon != null ? cannon : throw new ArgumentNullException(nameof(cannon));
            _effectSpeed = effectSpeed > 0 ? effectSpeed : throw new ArgumentOutOfRangeException(nameof(effectSpeed));
            _distance = distance;
            _height = heigth;
            _startPosition = cannon.position;
        }

        public void ReturnToNormalState()
        {
            _cannon.position = _startPosition;
            _cannon.rotation = Quaternion.identity;
        }

        public void Detonate()
        {
            Vector3 rotation = new (0, 0, 180);
            float half = 0.5f;
            Vector3 position = _cannon.position;
            float xPosition = position.x + _distance;
            float zPosition = position.z + (_distance * half);
            float yTopPosition = position.y + _height;
            float yBottomPosition = position.y - 1.5f;
            _cannon.DOMoveX(xPosition, _effectSpeed).SetEase(Ease.Linear);
            _cannon.DOMoveZ(zPosition, _effectSpeed).SetEase(Ease.Linear);
            Sequence ySequence = DOTween.Sequence();
            ySequence.Append(_cannon.DOMoveY(yTopPosition, _effectSpeed * half).SetEase(Ease.Linear));
            ySequence.Append(_cannon.DOMoveY(yBottomPosition, _effectSpeed * half).SetEase(Ease.Linear));
            int loops = 2;
            _cannon.DORotate(rotation, _effectSpeed * half).SetEase(Ease.Linear).SetLoops(loops, LoopType.Incremental);
        }
    }
}