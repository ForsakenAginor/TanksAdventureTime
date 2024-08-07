﻿using Enemies;
using Projectiles;
using System;
using UnityEngine;

namespace Assets.Source.LevelGeneration
{
    public class ObstaclesPoint : Point
    {
        [SerializeField] private Bomb[] _bombs;

        public void Init(IPlayerTarget player, Action<AudioSource> audioSourceAddedCallBack)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            IExplosive explosive = new Explosive(player);

            foreach (Bomb bomb in _bombs)
                bomb.Init(explosive, audioSourceAddedCallBack);
        }
    }
}