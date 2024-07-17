using Enemies;
using Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class MilitaryPoint : Point
    {
        [SerializeField] private EnemySetup[] _enemies;
        [SerializeField] private Bomb[] _bombs;
        private IPlayerTarget _player;

        public void Init(IPlayerTarget player, Action<AudioSource> audioSourceAddedCallBack, Action<IDamageableTarget> targetSpawnedCallback)
        {
            _player = player != null ? player : throw new ArgumentNullException(nameof(player));

            foreach (EnemySetup enemy in _enemies)
                enemy.Init(_player, audioSourceAddedCallBack, targetSpawnedCallback);

            IExplosive explosive = new Explosive(_player);

            foreach (Bomb bomb in _bombs)
                bomb.Init(explosive, audioSourceAddedCallBack);
        }
    }
}