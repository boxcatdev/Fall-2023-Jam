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
    [SerializeField] private AudioSource _hitSound;

    [SerializeField] private List<Image> _healthBats;


    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        RefreshUI();
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_hitSound != null) _hitSound.Play();
        if (_currentHealth <= 0)
        {
            RefreshUI();
            Destroy(gameObject);
            if(_deathEffect != null) Instantiate(_deathEffect, transform.position, Quaternion.identity);
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
        for (int i = 0; i < _healthBats.Count; i++)
        {
            if(i < _currentHealth)
            {
                _healthBats[i].gameObject.SetActive(true);
            }
            else
            {
                _healthBats[i].gameObject.SetActive(false);
            }
        }
    }
}