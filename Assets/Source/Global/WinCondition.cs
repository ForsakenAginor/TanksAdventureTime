using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Global
{
    public class WinCondition : MonoBehaviour
    {
        private IEnumerable<IDamageableTarget> _alivedEnemies;

        public event Action PlayerWon;

        private void FixedUpdate()
        {
            if (_alivedEnemies == null)
                return;

            if (_alivedEnemies.Count() == 0)
                PlayerWon?.Invoke();
        }

        public void Init(IEnumerable<IDamageableTarget> enemies)
        {
            _alivedEnemies = enemies != null ? enemies : throw new ArgumentNullException(nameof(enemies));
        }
    }
}