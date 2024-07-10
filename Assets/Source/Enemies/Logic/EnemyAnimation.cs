using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        private Animator _animator;

        public void Init(Animator animator, Action onInitializeCallback)
        {
            _animator = animator;
            onInitializeCallback?.Invoke();
            _animator.ResetTrigger(EnemyAnimations.Idle.ToString());
        }

        public void Play(EnemyAnimations animation)
        {
            _animator.SetTrigger(animation.ToString());
        }

        public bool IsPlaying()
        {
            return _animator.GetCurrentAnimatorStateInfo((int)ValueConstants.Zero).normalizedTime <=
                   (float)ValueConstants.One;
        }
    }
}