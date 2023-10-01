using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DigitalRuby.LightningBolt;

public class Chainable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private WaveManager waveManager;
    private Enemy enemy;

    //[SerializeField] Material defaultMat;
    //[SerializeField] Material hitMat;

    [SerializeField] GameObject lightningPrefab;

    [SerializeField] float hitRange = 3f;

    public bool hasBeenHit { get; private set; }

    //line rendering
    List<Vector3> startCoords = new List<Vector3>();
    List<Vector3> endCoords = new List<Vector3>();

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        waveManager = FindObjectOfType<WaveManager>();
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        
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
                        chainable.TriggerHit();
                        //line rendering
                        startCoords.Add(transform.position);
                        endCoords.Add(chainable.transform.position);
                    }
                }
            }
        }
    }

    public void TriggerHit()
    {

        hasBeenHit = true;

        //if (hitMat != null) meshRenderer.material = hitMat;

        DoHitCheck();

        DoHitEffect();
    }

    public void DoHitEffect()
    {
        enemy.UpdateEnemyState(EnemyState.Hurt);

        //create chain lightning effect
        for (int i = 0; i < startCoords.Count; i++)
        {
            GameObject lightningEffect = Instantiate(lightningPrefab, null);
             var lightning = lightningEffect.GetComponent<LightningBoltScript>();
             var lightningLine = lightningEffect.GetComponent<LineRenderer>();
            //trail.SetPosition(0, startCoords[i]);
            lightning.StartPosition = startCoords[i];
            //trail.SetPosition(1, endCoords[i]);
            lightning.EndPosition = endCoords[i];
            //Destroy(trail.gameObject, 1f);
            Destroy(lightningEffect, 1f);
        }

        //stun enemy temporarily before destroying
        Destroy(GetComponent<Enemy>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(gameObject, 1);

        //despawn enemy
        StartCoroutine(RemoveEnemy(0.96f));
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
