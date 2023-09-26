using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LassoGuided : MonoBehaviour
{
    [SerializeField] Transform _mouseTarget;
    [SerializeField] Transform _lassoVisual;
    [Space]
    public float lassoSize = 0.0f;
    public float maxSize = 5.0f;
    public float growRate = 0.01f;
    [Space]
    public bool isMovingLasso = false;
    public bool isChangingSize = false;

    private int count = 0;

    private StarterAssets.StarterAssetsInputs inputs;

    private void Awake()
    {
        inputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
    }
    private void Start()
    {
        RefreshVisuals();   
    }
    private void Update()
    {
        #region Lasso position
        //inputs
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (isChangingSize) return;

            //switch lasso bool
            isMovingLasso = !isMovingLasso;

            RefreshVisuals();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isMovingLasso) return;

            //switch changing bool
            isChangingSize = true;

            //start changing lasso size
            lassoSize = 0.01f;

            RefreshVisuals();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (!isMovingLasso) return;

            //switch changing bool
            isChangingSize = false;
            isMovingLasso = false;

            RefreshVisuals();
        }

        //change cursor state
        if (isMovingLasso)
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Confined;
            if (inputs.cursorInputForLook == true) inputs.cursorInputForLook = false;
        }
        else
        {
            if (Cursor.lockState == CursorLockMode.Confined) Cursor.lockState = CursorLockMode.Locked;
            if (inputs.cursorInputForLook == false) inputs.cursorInputForLook = true;
        }

        //enable/disable visual
        //RefreshVisuals();

        //move target to mouse position
        if (_lassoVisual != null && _mouseTarget != null)
        {
            if (isMovingLasso)
            {
                _lassoVisual.position = Utility.GetMouseHitPoint();
                _mouseTarget.position = Utility.GetMouseHitPoint();
            }
        }
        #endregion

        #region Lasso Size
        if (isChangingSize)
        {
            Debug.LogWarning("Resizing");
            lassoSize += growRate * Time.deltaTime;

            if(lassoSize >= maxSize)
            {

            }

            ScaleLasso(lassoSize);
        }
        #endregion
    }
    private void RefreshVisuals()
    {
        count++;
        Debug.LogFormat("RefreshVisuals() [{0}]", count);
        if (isMovingLasso == true && isChangingSize == false)
        {
            //show cursor hide lasso
            _mouseTarget.gameObject.SetActive(true);
            _lassoVisual.gameObject.SetActive(false);
        }
        else if (isMovingLasso == true && isChangingSize == true)
        {
            //hide cursor show lasso
            _mouseTarget.gameObject.SetActive(false);
            _lassoVisual.gameObject.SetActive(true);
        }
        else if(isMovingLasso == false)
        {
            //hide cursor hide lasso
            _mouseTarget.gameObject.SetActive(false);
            _lassoVisual.gameObject.SetActive(false);
        }
    }
    private void ToggleLasso()
    {

    }
    private void StartGrow()
    {

    }
    private void EndGrow()
    {

    }
    private void ScaleLasso(float scale)
    {
        Transform child = _lassoVisual.GetChild(0);

        Vector3 adjustedScale = new Vector3(scale, child.localScale.y, scale);
        child.localScale = adjustedScale;
    }
}
