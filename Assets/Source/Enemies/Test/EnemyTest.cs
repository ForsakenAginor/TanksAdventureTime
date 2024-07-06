using UnityEngine;

namespace Enemies
{
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] private TargetTest _target;
        [SerializeField] private EnemySetup[] _enemies;

        private void Awake()
        {
            foreach (EnemySetup enemy in _enemies)
                enemy.Init(_target);
        }
    }
}