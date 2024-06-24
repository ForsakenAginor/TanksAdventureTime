using UnityEngine;

namespace Enemies
{
    public class EnemySetup : MonoBehaviour
    {
        [SerializeField] private TargetTest _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _delay;

        private Transform _transform;
        private EnemyAnimation _animation;

        private FiniteStateMachine<EnemyState> _machine;
        private WaitForSeconds _wait;
    }
}