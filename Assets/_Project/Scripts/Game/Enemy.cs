using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Idle, Walk, Attack}
public class Enemy : MonoBehaviour
{
    public EnemyState enemyState { get; private set; }

    [SerializeField] private GameObject _lookAtPoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private int _attackRange;
    [SerializeField] private bool _isRanged;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private int _projectileSpeed;

    private GameObject _targetPlayer;
    private Health _playerHealth;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private float _currentDamageCooldown;
    private bool _onCooldown = false;

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            //need to make a enemies not get lasso'd inside of eachother
            _rb.Sleep();
        }
    }*/

/*    private void OnTriggerStay(Collider other)
    {
        if(!_onCooldown && !_isRanged && other.GetComponent<Health>())
        {
            _playerHealth.TakeDamage(_damage);
            _onCooldown = true;
        }
    }*/
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _targetPlayer = GameObject.FindGameObjectWithTag("Player");
        _playerHealth = _targetPlayer.GetComponent<Health>();
        _currentDamageCooldown = _attackSpeed;
    }
    private void Update()
    {
        if(_targetPlayer != null)
        {
            _navMeshAgent.destination = _targetPlayer.transform.position;
            _lookAtPoint.transform.position = _targetPlayer.transform.position + new Vector3(0,1,0);
            transform.LookAt(_lookAtPoint.transform.position);
        }

        if(_isRanged && Vector3.Distance(transform.position, _targetPlayer.transform.position) <= _attackRange)
        {
            if(_projectile != null && !_onCooldown)
            {
                var projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
                var projectileRb = projectile.GetComponent<Rigidbody>();
                projectileRb.AddForce(transform.forward * _projectileSpeed);
                _onCooldown = true;
                Destroy(projectile, _attackSpeed);
                projectileRb = null;
            }
        }

        if (_onCooldown)
        {
            _currentDamageCooldown -= Time.deltaTime;
            if (_currentDamageCooldown <= 0)
            {
                _onCooldown = false;
                _currentDamageCooldown = _attackSpeed;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
