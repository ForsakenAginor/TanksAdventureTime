using DG.Tweening;

namespace LearningGameMechanics
{
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

        public override void StartTraining()
        {
            DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                .Append(ImageTransform.DOLocalMoveX(FirstPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveX(-FirstPositionValue, SecondDuration))
                .Append(ImageTransform.DOLocalMoveX(SecondPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveY(FirstPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveY(-FirstPositionValue, SecondDuration))
                .Append(ImageTransform.DOLocalMoveY(SecondPositionValue, FirstDuration))
                .SetLoops(-SecondDuration);
        }
    }
}