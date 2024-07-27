using System;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour
{
    [SerializeField] private List<BunkerPart> _bunkerDetails;

    public event Action<float> TookDamage;
    public event Action<Action> Died;

    private float MaxHeath = 100;
    private float _currentHealth;
    private float _damage;


    private void Awake() => Init();

    private void OnEnable()
    {
        for (int i = 0; i < _bunkerDetails.Count; i++)
            _bunkerDetails[i].Destructed += TakeDamage;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _bunkerDetails.Count; i++)
            _bunkerDetails[i].Destructed -= TakeDamage;
    }

    private void Init()
    {
        _currentHealth = MaxHeath;
        _damage = _currentHealth / _bunkerDetails.Count;
        _damage = Mathf.CeilToInt(_damage);
    }

    private void TakeDamage(Action died)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= _damage;
            TookDamage?.Invoke(_currentHealth / MaxHeath);
        }

        if (_currentHealth <= 0)
            Died?.Invoke(died);
    }
}