using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteSwap : MonoBehaviour
{
    [SerializeField] float backAngle = 65f;
    [SerializeField] float sideAngle = 155f;

    private Camera mainCam;
    private NavMeshAgent agent;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Enemy enemy;

    private void Awake()
    {
        mainCam = Camera.main;
        agent = GetComponentInParent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemy = GetComponentInParent<Enemy>();
    }
    private void Update()
    {
        /*if (agent != null)
        {
            Debug.Log(agent.speed);
            //agent.
        }*/
    }
    private void LateUpdate()
    {
        if (agent == null) return;

        Vector3 camFowardVector = new Vector3(mainCam.transform.forward.x, 0f, mainCam.transform.forward.z);

        float signedAngle = Vector3.SignedAngle(agent.transform.forward, camFowardVector, Vector3.up);

        Vector2 animationDirection = new Vector2(0f, -1f);

        float angle = Mathf.Abs(signedAngle);

        if(angle < backAngle)
        {
            //back
            animationDirection = new Vector2(0f, -1f);
        }
        else if(angle < sideAngle)
        {
            //side

            if(signedAngle < 0)
            {
                animationDirection = new Vector2(-1f, 0f);
            }
            else
            {
                animationDirection = new Vector2(1f, 0f);
            }
            //animationDirection = new Vector2(1f, 0f);

            /*if (signedAngle < 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;*/
        }
        else
        {
            //front
            animationDirection = new Vector2(0f, 1f);
        }

        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}
