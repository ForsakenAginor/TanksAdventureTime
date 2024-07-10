using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] private TargetTest _target;
        [SerializeField] private EnemySetup[] _enemies;
        [SerializeField] private Bomb[] _bombs;

        private void Awake()
        {
            foreach (EnemySetup enemy in _enemies)
                enemy.Init(_target);

            IExplosive explosive = new Explosive(_target);

            foreach (Bomb bomb in _bombs)
                bomb.Init(explosive);
        }
    }
}