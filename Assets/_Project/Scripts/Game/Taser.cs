using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class Taser : MonoBehaviour
{
    private WaveManager waveManager;
    [SerializeField] AudioClip _taserSound;

    [SerializeField] float hitRange;

    private void Awake()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            DoHitCheck();
            if (_taserSound != null) Instantiate(_taserSound, transform.position, transform.rotation);
        }
    }

    public void DoHitCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out NavMeshAgent agent))
            {
                if (colliders[i].TryGetComponent(out Chainable chainable))
                {
                    if (chainable.hasBeenHit == false)
                    {
                        Debug.Log(name + " found: " + chainable.name);

                        chainable.TriggerHit();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}
