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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_canTase)
            {
                _canTase = false;

                DoHitCheck();
                if (_taserSound != null) Instantiate(_taserSound, transform.position, transform.rotation);
            }
        }

        #region Cooldown

        if (_canTase == false)
        {
            if (_cooldownStarted == false)
            {
                //start cooldown
                _cooldownStarted = true;
                _countdown = _cooldownTime;
            }

            if (_countdown >= 0)
            {
                _countdown -= Time.deltaTime;
            }
            else
            {
                if (_cooldownStarted == true && _canTase == false)
                {
                    //end lasso
                    _canTase = true;
                    _cooldownStarted = false;
                }
            }

            RefreshCooldownDisplay();
        }

        #endregion
    }

    private void RefreshCooldownDisplay()
    {
        if (_cooldownDisplay != null)
        {
            _cooldownDisplay.fillAmount = 1 - (_countdown / _cooldownTime);
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
