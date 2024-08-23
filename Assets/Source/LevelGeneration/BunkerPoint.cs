using Enemies;
using System;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class BunkerPoint : Point
    {
        [SerializeField] private EnemySetup _bunker;

        public void Init(IPlayerTarget player, Action<AudioSource> audioSourceAddedCallBack, Action<IDamageableTarget> targetSpawnedCallback)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            _bunker.Init(player, audioSourceAddedCallBack, targetSpawnedCallback);
        }
    }
}