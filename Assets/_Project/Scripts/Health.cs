using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private ParticleSystem _deathEffect;

    [SerializeField] private int _currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if(_currentHealth <= 0)
        {
            Destroy(gameObject);
            if(_deathEffect != null)
            {
                Instantiate(_deathEffect, transform.position, Quaternion.identity);
            }
        }
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
    }

    public int GetHealth()
    {
        return _currentHealth;
    }
}
