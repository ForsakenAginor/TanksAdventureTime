using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DestructionObject
{
    public class Bunker : MonoBehaviour, IDamageable
    {
        private const int MinValue = 0;

        [SerializeField] private List<BunkerPart> _bunkerDetails;

        private float _maxHeath = 100;
        private float _currentHealth;
        private float _damage;

        public event Action<float> DamageTaking;

        public event Action Died;

        public Bunker Init(float maxHealth)
        {
            _maxHeath = maxHealth;
            _currentHealth = _maxHeath;
            _damage = Mathf.CeilToInt(_currentHealth / _bunkerDetails.Count);
            return this;
        }

        public void TakeDamage(int value)
        {
            List<BunkerPart> bunkers = TryGetNoDestructionPart();

            if (bunkers.Count == MinValue)
            {
                Died?.Invoke();
                return;
            }

            _currentHealth -= _damage;
            bunkers[GetRandomPart(bunkers.Count)].React();
            DamageTaking?.Invoke(_currentHealth / _maxHeath);
        }

        private List<BunkerPart> TryGetNoDestructionPart()
        {
            return _bunkerDetails.Where(part => part.IsDestroyed == false).ToList();
        }

        private int GetRandomPart(int countBunkerPart)
        {
            return UnityEngine.Random.Range(MinValue, countBunkerPart);
        }
    }
}