using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private ParticleSystem _deathEffect;
    [SerializeField] private AudioClip _hitSound;


    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;

        RefreshUI();
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if(_currentHealth <= 0)
        {
            Destroy(gameObject);
            if(_deathEffect != null) Instantiate(_deathEffect, transform.position, Quaternion.identity);
            if (_hitSound != null) Instantiate(_hitSound, transform.position, transform.rotation);
        }

        RefreshUI();
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;

        RefreshUI();
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    private void RefreshUI()
    {
        if (_healthSlider)
        {
            if (_healthSlider.maxValue != _maxHealth) _healthSlider.maxValue = _maxHealth;

            if (_healthSlider.value != _currentHealth) _healthSlider.value = _currentHealth;
        }
    }
}
