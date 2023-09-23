using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoProjectile : MonoBehaviour
{
    [SerializeField] int _lassoRange;
    [SerializeField] int _pullForce;

    private Vector3 _pullTowards;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ground")
        {
            _pullTowards = transform.position;
            Collider[] _enemiesInRange = Physics.OverlapSphere(transform.position, _lassoRange);
            for (int i = 0; i < _enemiesInRange.Length; i++)
            {
                if(_enemiesInRange[i].gameObject.TryGetComponent(out Chainable chainable))
                {
                    _enemiesInRange[i].transform.position = Vector3.MoveTowards(_enemiesInRange[i].transform.position, _pullTowards, _pullForce * Time.deltaTime);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _lassoRange);
    }
}
