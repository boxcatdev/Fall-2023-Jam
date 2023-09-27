using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chainable : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private WaveManager waveManager;

    [SerializeField] Material defaultMat;
    [SerializeField] Material hitMat;

    [SerializeField] float hitRange = 3f;

    public bool canTriggerHits = false;
    public bool hasBeenHit = false;

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

    private void Update()
    {
        if (canTriggerHits)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                DoHitCheck();
            }
        }
    }
    private void LateUpdate()
    {
        if(hasBeenHit && !canTriggerHits)
        {
            Destroy(GetComponent<Enemy>());
            Destroy(gameObject, 1);
        }
    }

    public void DoHitCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitRange);

        startCoords = new List<Vector3>();
        endCoords = new List<Vector3>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Chainable chainable))
            {
                if (chainable != this && chainable.hasBeenHit == false)
                {
                    Debug.Log(name + " found: " + chainable.name);

                    chainable.GetHit();

                    //line rendering
                    startCoords.Add(transform.position); //if(!startCoords.Contains(transform.position)) 
                    endCoords.Add(chainable.transform.position); //if(!endCoords.Contains(chainable.transform.position)) 
                }
            }
        }
    }

    public void GetHit()
    {
        hasBeenHit = true;

        if (hitMat != null) meshRenderer.material = hitMat;

        if(waveManager != null) waveManager.RemoveEnemy();

        DoHitCheck();
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
