using System;
using System.Collections.Generic;
using System.Linq;

public class EnemiesManager
{
    private IEnumerable<IDamageableTarget> _enemies;

    public EnemiesManager(IEnumerable<IDamageableTarget> enemies)
    {
        _enemies = enemies != null ? enemies : throw new ArgumentNullException(nameof(enemies));
    }

    public IEnumerable<IDamageableTarget> AlivedEnemies => _enemies.Where(o => o.Priority != TargetPriority.None);
}
