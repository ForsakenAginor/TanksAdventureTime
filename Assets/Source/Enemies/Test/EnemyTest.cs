using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimation))]
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] private TargetTest _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _head;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _fsmUpdateDelay;
        [SerializeField] private LayerMask _walls;

        private Transform _transform;
        private EnemyAnimation _animation;

        private FiniteStateMachine<EnemyState> _machine;
        private IFieldOfView _fieldOfView;
        private EnemyRotator _rotator;
        private CancellationToken _token;

        private void OnDrawGizmos()
        {
            if (_head == null)
                return;

            if (_target == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_head.position, _attackRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_head.position, _target.transform.position);
        }

        private void OnDestroy()
        {
            _machine.Exit();
        }

        private void Start()
        {
            _animation = GetComponent<EnemyAnimation>();

            _machine = new FiniteStateMachine<EnemyState>();
            _fieldOfView = new EnemyFieldOfView(_target, _head, _attackRadius, _walls);
            _rotator = new EnemyRotator(_rotationSpeed, _transform, _target);
            _token = destroyCancellationToken;
            _machine.AddStates(
                new Dictionary<Type, FiniteStateMachineState<EnemyState>>()
                {
                    {
                        typeof(EnemyIdleState),
                        new EnemyIdleState(_machine, _animation, _fieldOfView)
                    },
                    {
                        typeof(EnemyAttackState),
                        new EnemyAttackState(_machine, _animation, _fieldOfView, _rotator)
                    },
                });

            _animation.Init(_animator, () => _machine.SetState(typeof(EnemyIdleState)));
            UpdateRoutine().Forget();
        }

        private async UniTaskVoid UpdateRoutine()
        {
            while (_token.IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_fsmUpdateDelay), cancellationToken: _token);
                _machine.Update();
            }
        }
    }
}