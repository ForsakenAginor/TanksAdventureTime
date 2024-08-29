using DG.Tweening;

namespace LearningGameMechanics
{
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

        protected override void OnTrainingStart()
        {
            int endValue = 50;
            int endValue1 = 0;
            float duration = 0.5f;
            int duration1 = 1;
            DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                             .Append(ImageTransform.DOLocalMoveY(-endValue, duration))
                             .Append(ImageTransform.DOLocalMoveY(endValue1, duration))
                             .SetLoops(-duration1);
        }
    }
}