using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private IEnumerable<IDamageableTarget> _aliveEnemies;

    public event Action PlayerWon;

    private void FixedUpdate()
    {
        if (_aliveEnemies == null)
            return;

        if (_aliveEnemies.Any() == false)
            PlayerWon?.Invoke();
    }

    public void Init(IEnumerable<IDamageableTarget> enemies)
    {
        _aliveEnemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
    }
}