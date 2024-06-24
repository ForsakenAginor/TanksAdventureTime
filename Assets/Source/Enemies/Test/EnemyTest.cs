using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimation))]
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] private TargetTest _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _delay;
        [SerializeField] private Button _button;

        private Transform _transform;
        private EnemyAnimation _animation;

        private FiniteStateMachine<EnemyState> _machine;
        private WaitForSeconds _wait;

        private void OnDrawGizmos()
        {
            if (_target == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, _target.transform.position);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(PPP);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PPP);
        }

        private void PPP()
        {
            print("Я в порядке");
        }

        private async void Start()
        {
            _transform = transform;
            _animation = GetComponent<EnemyAnimation>();

            _machine = new FiniteStateMachine<EnemyState>();
            _wait = new WaitForSeconds(_delay);
            _machine.AddStates(
                new Dictionary<Type, FiniteStateMachineState<EnemyState>>()
                {
                    {
                        typeof(EnemyIdleState),
                        new EnemyIdleState(_machine, _animation, _target, _transform, _attackRadius, _rotationSpeed)
                    },
                    {
                        typeof(EnemyAttackState),
                        new EnemyAttackState(_machine, _animation, _target, _transform, _attackRadius, _rotationSpeed)
                    },
                });

            _animation.Init(_animator, () => _machine.SetState(typeof(EnemyIdleState)));
        }

        private async UniTask UpdateRoutine()
        {
            while (true)
            {
                await UniTask.Delay(1);
                _machine.Update();
            }
        }
    }
}