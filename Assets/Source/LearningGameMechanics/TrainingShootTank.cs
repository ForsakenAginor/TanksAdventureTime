using DG.Tweening;

public class TrainingShootTank : Training
{
    private void OnEnable()
    {
        InputSystem.Player.Fire.started += OnInputActive;
        InputSystem.Player.Fire.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Fire.started -= OnInputActive;
        InputSystem.Player.Fire.canceled -= OnCanceled;
    }

    protected override void TrainingStart()
    {
        DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                         .Append(ImageTransform.DOLocalMoveY(-50, 0.5f))
                         .Append(ImageTransform.DOLocalMoveY(0, 0.2f))
                         .SetLoops(-1);
    }
}