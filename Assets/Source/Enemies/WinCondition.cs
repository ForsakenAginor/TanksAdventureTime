using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private IEnumerable<IDamageableTarget> _alivedEnemies;
    private bool _isPlayerWon;

    public event Action PlayerWon;

    private void FixedUpdate()
    {
        if (_alivedEnemies == null)
            return;

        if(_isPlayerWon == false && _alivedEnemies.Count() == 0)
        {
            _isPlayerWon = true;
            PlayerWon?.Invoke();
        }
    }

    public void Init(IEnumerable<IDamageableTarget> enemies)
    {
        _alivedEnemies = enemies != null ? enemies : throw new ArgumentNullException(nameof(enemies));
    }
}