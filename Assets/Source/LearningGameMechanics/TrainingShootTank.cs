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

        public override void StartTraining()
        {
            DOTween.Sequence().SetUpdate(UpdateType.Normal, true)
                .Append(ImageTransform.DOLocalMoveY(-FirstPositionValue, FirstDuration))
                .Append(ImageTransform.DOLocalMoveY(SecondPositionValue, FirstDuration))
                .SetLoops(-SecondDuration);
        }
    }
}