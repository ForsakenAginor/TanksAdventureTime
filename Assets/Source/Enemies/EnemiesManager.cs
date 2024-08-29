using System;
using System.Collections.Generic;
using System.Linq;

namespace Enemies
{
    public class EnemiesManager
    {
        private readonly IEnumerable<IDamageableTarget> _enemies;

        public EnemiesManager(IEnumerable<IDamageableTarget> enemies)
        {
            _enemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
        }

        public IEnumerable<IDamageableTarget> AliveEnemies => _enemies.Where(o => o.Priority != TargetPriority.None);
    }
}