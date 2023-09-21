using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainable : MonoBehaviour
{
    private void Start()
    {
        //Debug.Log(name);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("text");

        Chainable chainable = other.GetComponent<Chainable>();
        if (chainable != null)
        {
            Debug.Log(chainable.name);
        }
    }
}
