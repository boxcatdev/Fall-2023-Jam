using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Taser : MonoBehaviour
{
    private WaveManager waveManager;

    [SerializeField] float hitRange = 3f;

    private void Awake()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            DoHitCheck();
        }
    }

    public void DoHitCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Chainable chainable))
            {
                if (chainable != this && chainable.hasBeenHit == false)
                {
                    Debug.Log(name + " found: " + chainable.name);

                    chainable.TriggerHit();
                }
            }
        }
    }
}
