using System;
using System.Collections.Generic;
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

        public event Action<float> TookDamage;

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
            TookDamage?.Invoke(_currentHealth / _maxHeath);
        }

        private List<BunkerPart> TryGetNoDestructionPart()
        {
            List<BunkerPart> bunkerParts = new ();

            foreach (var part in _bunkerDetails)
                if (part.IsDestroyed == false)
                    bunkerParts.Add(part);

            return bunkerParts;
        }

        private int GetRandomPart(int countBunkerPart)
        {
            return UnityEngine.Random.Range(MinValue, countBunkerPart);
        }
    }
}