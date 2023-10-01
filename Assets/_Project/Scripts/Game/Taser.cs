using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;

public class Taser : MonoBehaviour
{
    private WaveManager waveManager;
    [SerializeField] AudioClip _taserSound;
    [SerializeField] Image _cooldownDisplay;

    [SerializeField] float hitRange;
    [SerializeField] private float _cooldownTime;

    [SerializeField] private bool _canTase;
    private bool _cooldownStarted;
    private float _countdown;

    public List<Collider> collidersInRange = new List<Collider>();

    private SphereCollider sphereCollider;

    private void Awake()
    {
        waveManager = FindObjectOfType<WaveManager>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        sphereCollider.radius = hitRange;
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
        foreach (var collider in collidersInRange)
        {
            if (collider.TryGetComponent(out Chainable chainable))
            {
                if (chainable.hasBeenHit == false)
                {
                    Debug.Log(name + " found: " + chainable.name);

                    chainable.TriggerHit();
                }
            }
        }
        

        /*Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * 1 , hitRange);

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
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Chainable chainable))
        {
            Debug.Log("chainable added");
            if(!collidersInRange.Contains(other))
                collidersInRange.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Chainable chainable))
        {
            if (collidersInRange.Contains(other))
                collidersInRange.Remove(other);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}
