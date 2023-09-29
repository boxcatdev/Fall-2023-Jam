using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChainableWithNavmesh : MonoBehaviour
{
    [SerializeField] private float hitRange = 5f;
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitRange);

        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out NavMeshAgent agent))
            {
                Debug.Log(agent.name + " has NavMeshAgent");
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}
