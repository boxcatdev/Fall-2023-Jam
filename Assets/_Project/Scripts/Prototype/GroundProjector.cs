using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundProjector : MonoBehaviour
{
    Transform parentTransform;

    private void Awake()
    {
        parentTransform = transform.parent;
    }
    private void Update()
    {
        if (Physics.Raycast(parentTransform.position + (Vector3.up * 1f), Vector3.down, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }
}
