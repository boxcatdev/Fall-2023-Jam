using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private int _healthGained;
    [SerializeField] private GameObject _pickUpEffect;
    [SerializeField] private GameObject _orbToDisable;
    [SerializeField] private AudioSource _pickUpSound;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Vector3 _rotationDirection;

    private Collider _collider;
    private MeshRenderer _artToDisable;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _artToDisable = GetComponent<MeshRenderer>();
        _pickUpSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if(health != null)
        {
            //soft disable
            _collider.enabled = false;
            _artToDisable.enabled = false;
            _orbToDisable.SetActive(false);

            health.Heal(_healthGained);
            if (_pickUpEffect != null) Instantiate(_pickUpEffect, transform.position, transform.rotation);
            if (_pickUpSound != null) _pickUpSound.Play();

            Destroy(gameObject, 2f);
        }
    }

    private void Update()
    {
        transform.Rotate(_rotateSpeed * _rotationDirection * Time.deltaTime);
    }
}
