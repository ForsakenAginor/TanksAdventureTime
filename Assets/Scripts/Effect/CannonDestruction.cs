using UnityEngine;
using DG.Tweening;
using System;

public class CannonDestruction : IOnDeathEffect
{
    private readonly Transform _cannon;
    private readonly float _effectSpeed;
    private readonly float _distance;
    private readonly float _heigth;
    private readonly Vector3 _startPosition;

    public CannonDestruction(Transform cannon, float effectSpeed, float distance, float heigth)
    {
        _cannon = cannon != null ? cannon : throw new ArgumentNullException(nameof(cannon));
        _effectSpeed = effectSpeed > 0 ? _effectSpeed : throw new ArgumentOutOfRangeException(nameof(effectSpeed));
        _distance = distance;
        _heigth = heigth;
        _startPosition = cannon.position;
    }

    public void ReturnToNormalState()
    {
        _cannon.position = _startPosition;
        _cannon.rotation = Quaternion.identity;
    }

    public void Detonate()
    {
        Vector3 rotation = new Vector3(0, 0, 180);
        float half = 0.5f;
        float xPosition = _cannon.position.x + _distance;
        float zPosition = _cannon.position.z + _distance * half;
        float yTopPosition = _cannon.position.y + _heigth;
        float yBottomPosition = _cannon.position.y - 1.5f;
        _cannon.DOMoveX(xPosition, _effectSpeed).SetEase(Ease.Linear);
        _cannon.DOMoveZ(zPosition, _effectSpeed).SetEase(Ease.Linear);
        Sequence ySequence = DOTween.Sequence();
        ySequence.Append(_cannon.DOMoveY(yTopPosition, _effectSpeed * half).SetEase(Ease.Linear));
        ySequence.Append(_cannon.DOMoveY(yBottomPosition, _effectSpeed * half).SetEase(Ease.Linear));
        int loops = 2;
        _cannon.DORotate(rotation, _effectSpeed * half).SetEase(Ease.Linear).SetLoops(loops, LoopType.Incremental);
    }
}

