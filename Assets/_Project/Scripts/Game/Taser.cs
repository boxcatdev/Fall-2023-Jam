using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;

public class Taser : MonoBehaviour
{
    private WaveManager waveManager;
    [SerializeField] AudioSource _taserSound;
    [SerializeField] Image _cooldownDisplay;

    [SerializeField] float _hitRange;
    [SerializeField] float _detectionRange = 50f;
    [SerializeField] private float _cooldownTime;

    [SerializeField] private bool _canTase;
    private bool _cooldownStarted;
    private float _countdown;

    //public List<Collider> collidersInRange = new List<Collider>();

    //private SphereCollider sphereCollider;

    private void Awake()
    {
        waveManager = FindObjectOfType<WaveManager>();
        //sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        //sphereCollider.radius = _hitRange;
    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_canTase)
            {
                _canTase = false;

                DoHitCheck();
                //if (_taserSound != null) Instantiate(_taserSound, transform.position, transform.rotation);
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
        /*foreach (var collider in collidersInRange)
        {
            if (collider.TryGetComponent(out Chainable chainable))
            {
                if (chainable.hasBeenHit == false)
                {
                    Debug.Log(name + " found: " + chainable.name);

                    collidersInRange.Remove(collider);

                    chainable.TriggerHit(this);
                }
            }
        }*/
        /*for (int i = 0; i < colliders.Length; i++)
        {

        }*/

        int chainFound = 0;
        int inRange = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * 1, _hitRange);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Chainable chainable))
            {
                chainFound++;

                inRange++;

                if (chainable.hasBeenHit == false)
                {
                    //Debug.Log(name + " found: " + chainable.name);
                    chainable.TriggerHit();
                    if(_taserSound.isPlaying == false)
                    {
                        //if (_taserSound != null) Instantiate(_taserSound, transform.position, transform.rotation);
                        _taserSound.Play();
                    }
                }

                /*if (Vector3.Distance(transform.position, collider.transform.position) <= _hitRange)
                {
                    inRange++;

                    if (chainable.hasBeenHit == false)
                    {
                        //Debug.Log(name + " found: " + chainable.name);


                        chainable.TriggerHit();
                    }
                }*/
            }
        }

        //Debug.LogFormat("Found: {0}; In Range: {1}", chainFound, inRange);
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Chainable chainable))
        {
            Debug.Log("chainable added");
            if (!collidersInRange.Contains(other))
                collidersInRange.Add(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Chainable chainable))
        {
            if (collidersInRange.Contains(other))
                collidersInRange.Remove(other);
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, _hitRange * 0.75f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, _hitRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 1f, _detectionRange);
    }
}
