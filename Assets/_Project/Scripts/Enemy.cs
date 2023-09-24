using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageCooldown;
    [SerializeField] private int _attackRange;

    private GameObject _targetPlayer;
    private Health _playerHealth;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private float _currentDamageCooldown;
    private bool _onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            _rb.Sleep();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!_onCooldown && other.GetComponent<Lasso>())
        {
            _playerHealth.TakeDamage(_damage);
            _onCooldown = true;
        }
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _targetPlayer = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _targetPlayer.GetComponent<Health>();
        _currentDamageCooldown = _damageCooldown;
    }
    private void Update()
    {
        if(_targetPlayer != null)
        {
            _navMeshAgent.destination = _targetPlayer.transform.position;
        }
        if (_onCooldown)
        {
            _currentDamageCooldown -= Time.deltaTime;
            if (_currentDamageCooldown <= 0)
            {
                _onCooldown = false;
                _currentDamageCooldown = _damageCooldown;
            }
        }
    }
}
