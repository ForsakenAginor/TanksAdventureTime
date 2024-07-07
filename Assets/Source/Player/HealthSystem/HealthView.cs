﻿using System;
using UnityEngine;

namespace Assets.Source.Player.HealthSystem
{
    public abstract class HealthView : MonoBehaviour
    {
        private Health _health;

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        public virtual void Init(Health health)
        {
            _health = health != null ? health : throw new ArgumentNullException(nameof(health));
            _health.HealthChanged += OnHealthChanged;
            OnHealthChanged(health.Current, health.Maximum);
        }

        protected abstract void OnHealthChanged(int current, int max);
    }
}