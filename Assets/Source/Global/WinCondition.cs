using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private IEnumerable<IDamageableTarget> _aliveEnemies;

    private bool _isPlayerWon;

    public event Action PlayerWinning;

    private void FixedUpdate()
    {
        if (_aliveEnemies == null)
            return;

        if (_isPlayerWon == false && _aliveEnemies.Any() == false)
        {
            _isPlayerWon = true;
            PlayerWinning?.Invoke();
        }
    }

    public void Init(IEnumerable<IDamageableTarget> enemies)
    {
        _aliveEnemies = enemies ?? throw new ArgumentNullException(nameof(enemies));
    }
}