using System;
using System.Linq;
using Enemies;
using UnityEngine;

namespace LevelGeneration
{
    public class BunkerPoint : Point
    {
        [SerializeField] private EnemySetup[] _bunkers;

        public void Init(
            IPlayerTarget player,
            Action<AudioSource> audioSourceAddedCallBack,
            Action<IDamageableTarget> targetSpawnedCallback)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            _bunkers.ToList().ForEach(o => o.Init(player, audioSourceAddedCallBack, targetSpawnedCallback));
        }
    }
}