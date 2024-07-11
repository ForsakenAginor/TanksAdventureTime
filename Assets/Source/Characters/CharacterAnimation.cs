using System;
using UnityEngine;

namespace Characters
{
    public class CharacterAnimation : MonoBehaviour
    {
        private Animator _animator;

        public void Init(Animator animator, Action onInitializeCallback)
        {
            _animator = animator;
            onInitializeCallback?.Invoke();
            _animator.ResetTrigger(CharacterAnimations.Idle.ToString());
        }

        public void Play(CharacterAnimations animation)
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