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

        protected override void OnTrainingStart()
        {
            int endValue = 50;
            int endValue1 = 0;
            float duration = 0.5f;
            int duration1 = 1;
            DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                             .Append(ImageTransform.DOLocalMoveX(endValue, duration))
                             .Append(ImageTransform.DOLocalMoveX(-endValue, duration1))
                             .Append(ImageTransform.DOLocalMoveX(endValue1, duration))
                             .Append(ImageTransform.DOLocalMoveY(endValue, duration))
                             .Append(ImageTransform.DOLocalMoveY(-endValue, duration1))
                             .Append(ImageTransform.DOLocalMoveY(endValue1, duration))
                             .SetLoops(-duration1);
        }
    }
}