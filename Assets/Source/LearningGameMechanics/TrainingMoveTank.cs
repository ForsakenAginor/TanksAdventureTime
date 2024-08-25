using DG.Tweening;

public class TrainingMoveTank : Training
{
    private void OnEnable()
    {
        InputSystem.Player.Move.started += OnInputActive;
        InputSystem.Player.Move.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        InputSystem.Player.Move.started -= OnInputActive;
        InputSystem.Player.Move.canceled -= OnCanceled;
    }

    protected override void TrainingStart()
    {
        DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                          .Append(ImageTransform.DOLocalMoveY(50, 0.5f))
                          .Append(ImageTransform.DOLocalMoveY(-50, 1))
                          .Append(ImageTransform.DOLocalMoveY(0, 0.5f))
                          .Append(ImageTransform.DOLocalMoveX(50, 0.5f))
                          .Append(ImageTransform.DOLocalMoveX(-50, 1))
                          .Append(ImageTransform.DOLocalMoveX(0, 0.5f))
                          .SetLoops(-1);
    }
}