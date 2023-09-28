using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chainable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private WaveManager waveManager;

    [SerializeField] Material defaultMat;
    [SerializeField] Material hitMat;

    [SerializeField] LineRenderer lightningPrefab;

    [SerializeField] float hitRange = 3f;

    public bool hasBeenHit { get; private set; }

    //line rendering
    List<Vector3> startCoords = new List<Vector3>();
    List<Vector3> endCoords = new List<Vector3>();

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        waveManager = FindObjectOfType<WaveManager>();
    }
    private void Start()
    {
        if(!hasBeenHit)
            if(defaultMat != null) meshRenderer.material = defaultMat;
        else
            if (hitMat != null) meshRenderer.material = hitMat;
    }

    private void LateUpdate()
    {
        /*if(hasBeenHit)
        {
            Destroy(GetComponent<Enemy>());
            Destroy(gameObject, 1);
        }*/
    }

    public void DoHitCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitRange);

        startCoords = new List<Vector3>();
        endCoords = new List<Vector3>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].TryGetComponent(out NavMeshAgent agent))
            {
                if(colliders[i].TryGetComponent(out Chainable chainable))
                {
                    if (chainable != this && chainable.hasBeenHit == false)
                    {
                        //Debug.Log(name + " found: " + chainable.name);

                        chainable.TriggerHit();

                        //line rendering
                        startCoords.Add(transform.position);
                        endCoords.Add(chainable.transform.position);
                    }
                }
            }

            /*if (colliders[i].TryGetComponent(out Chainable chainable))
            {
                if (chainable != this && chainable.hasBeenHit == false)
                {
                    Debug.Log(name + " found: " + chainable.name);

                    chainable.TriggerHit();

                    //line rendering
                    startCoords.Add(transform.position); //if(!startCoords.Contains(transform.position)) 
                    endCoords.Add(chainable.transform.position); //if(!endCoords.Contains(chainable.transform.position)) 
                }
            }*/
        }
    }

    public void TriggerHit()
    {
        hasBeenHit = true;

        if (hitMat != null) meshRenderer.material = hitMat;

        DoHitCheck();

        DoHitEffect();
    }

    public void DoHitEffect()
    {
        //create chain lightning effect
        for (int i = 0; i < startCoords.Count; i++)
        {
            LineRenderer trail = Instantiate(lightningPrefab, null);
            trail.SetPosition(0, startCoords[i]);
            trail.SetPosition(1, endCoords[i]);
            Destroy(trail.gameObject, 1f);
        }

        //stun enemy temporarily before destroying
        Destroy(GetComponent<Enemy>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(gameObject, 1);

        //despawn enemy
        StartCoroutine(RemoveEnemy(0.99f));
    }

    IEnumerator RemoveEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (waveManager != null) waveManager.RemoveEnemy();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if(!hasBeenHit)
            Gizmos.DrawWireSphere(transform.position, hitRange);

        Gizmos.color = Color.red;
        if(startCoords.Count > 0)
        {
            for (int i = 0; i < startCoords.Count; i++)
            {
                Gizmos.DrawLine(startCoords[i], endCoords[i]);
            }
        }
        
    }
}
