using DG.Tweening;

public class TrainingRotateTank : Training
{
    private void OnEnable()
    {
        InputSystem.Player.Rotate.started += OnInputActive;
        InputSystem.Player.Rotate.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Rotate.started -= OnInputActive;
        InputSystem.Player.Rotate.canceled -= OnCanceled;
    }

    protected override void TrainingStart()
    {
        DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                         .Append(ImageTransform.DOLocalMoveX(50, 0.5f))
                         .Append(ImageTransform.DOLocalMoveX(-50, 1))
                         .Append(ImageTransform.DOLocalMoveX(0, 0.5f))
                         .Append(ImageTransform.DOLocalMoveY(50, 0.5f))
                         .Append(ImageTransform.DOLocalMoveY(-50, 1))
                         .Append(ImageTransform.DOLocalMoveY(0, 0.5f))
                         .SetLoops(-1);
    }
}