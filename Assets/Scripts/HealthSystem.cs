using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnEventDead;
    public event EventHandler OnEventDamaged;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health = 100;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        OnEventDamaged?.Invoke(this, EventArgs.Empty);
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    private void Die()
    {
        OnEventDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetNormalizedHealth()
    {
        return (float)_health / _maxHealth;
    }
}