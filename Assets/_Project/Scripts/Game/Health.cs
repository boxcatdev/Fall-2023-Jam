using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private ParticleSystem _deathEffect;
    [SerializeField] private AudioSource _hitSound;

    [SerializeField] private List<Image> _healthBats;

    public UnityEvent OnDeath;

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
            //Destroy(gameObject);

            OnDeath?.Invoke();

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

        /*if (_healthSlider)
        {
            if (_healthSlider.maxValue != _maxHealth) _healthSlider.maxValue = _maxHealth;

            if (_healthSlider.value != _currentHealth) _healthSlider.value = _currentHealth;
        }*/
    }
}
