using DG.Tweening;

namespace LearningGameMechanics
{
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

        public override void StartTraining()
        {
            DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                .Append(ImageTransform.DOLocalMoveY(FirstPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveY(-FirstPositionValue, SecondDuration))
                .Append(ImageTransform.DOLocalMoveY(SecondPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveX(FirstPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveX(-FirstPositionValue, SecondDuration))
                .Append(ImageTransform.DOLocalMoveX(SecondPositionValue, FirstDuration))
                .SetLoops(-SecondDuration);
        }
    }
}