using Enemies;
using Projectiles;
using System;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class MilitaryPoint : Point
    {
        [SerializeField] private EnemySetup[] _enemies;
        [SerializeField] private Bomb[] _bombs;

        public void Init(IPlayerTarget player, Action<AudioSource> audioSourceAddedCallBack, Action<IDamageableTarget> targetSpawnedCallback)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            foreach (EnemySetup enemy in _enemies)
                enemy.Init(player, audioSourceAddedCallBack, targetSpawnedCallback);

            IExplosive explosive = new Explosive(player);

            foreach (Bomb bomb in _bombs)
                bomb.Init(explosive, audioSourceAddedCallBack);
        }
    }
}