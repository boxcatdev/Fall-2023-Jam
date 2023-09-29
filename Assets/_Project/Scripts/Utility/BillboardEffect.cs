using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        #region Update Direction

        Vector3 camPosition = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        Vector3 targetDirection = camPosition - transform.position;

        transform.forward = targetDirection;

        #endregion
    }
}
