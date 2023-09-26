using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LassoGuided : MonoBehaviour
{
    [SerializeField] Transform _mouseTarget;
    [SerializeField] Transform _lassoVisual;

    public bool canMoveLasso = false;

    private StarterAssets.StarterAssetsInputs inputs;

    private void Awake()
    {
        inputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
    }
    private void Start()
    {
        /*if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }*/
    }
    private void Update()
    {
        //inputs
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            //switch lasso bool
            canMoveLasso = !canMoveLasso;

            if (canMoveLasso)
            {
                Cursor.lockState = CursorLockMode.Confined;
                inputs.cursorInputForLook = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                inputs.cursorInputForLook = true;
            }

            /*if (canMoveLasso)
                inputs.cursorInputForLook = false;
            else*/

        }

        //enable/disable visual
        if(_lassoVisual != null)
        {
            if (canMoveLasso == false)
                if (_lassoVisual.gameObject.activeInHierarchy) _lassoVisual.gameObject.SetActive(false);
            if (canMoveLasso == true)
                if (!_lassoVisual.gameObject.activeInHierarchy) _lassoVisual.gameObject.SetActive(true);
        }
        if (_mouseTarget != null)
        {
            if (canMoveLasso == false)
                if (_mouseTarget.gameObject.activeInHierarchy) _mouseTarget.gameObject.SetActive(false);
            if (canMoveLasso == true)
                if (!_mouseTarget.gameObject.activeInHierarchy) _mouseTarget.gameObject.SetActive(true);
        }

        //move target to mouse position
        if (_lassoVisual != null && _mouseTarget != null)
        {
            if (canMoveLasso)
            {
                _lassoVisual.position = Utility.GetMouseHitPoint();
                _mouseTarget.position = Utility.GetMouseHitPoint();
            }
        }
    }
}
