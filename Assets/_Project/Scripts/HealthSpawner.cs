using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private GameObject _healthPickUp;


    private float _currentCooldown;
    private GameObject _currentPickUp;

    private void Start()
    {
        _currentCooldown = _spawnCooldown;
        _currentPickUp = Instantiate(_healthPickUp, transform.position, transform.rotation);
    }
    void Update()
    {
        if(_currentPickUp == null)
        {
            if (_currentCooldown <= 0)
            {
                _currentPickUp = Instantiate(_healthPickUp, transform.position, transform.rotation);
                _currentCooldown = _spawnCooldown;
            }
            else
            {
                _currentCooldown -= Time.deltaTime;
            }
        }
    }
}
