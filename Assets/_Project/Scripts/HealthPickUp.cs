using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private int _healthGained;
    [SerializeField] private ParticleSystem _pickUpEffect;
    [SerializeField] private AudioSource _pickUpSound;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if(health != null)
        {
            health.Heal(_healthGained);
            if (_pickUpEffect != null) Instantiate(_pickUpEffect, transform.position, transform.rotation);
            if (_pickUpSound != null) _pickUpSound.Play();
            Destroy(gameObject);
        }
    }
}
