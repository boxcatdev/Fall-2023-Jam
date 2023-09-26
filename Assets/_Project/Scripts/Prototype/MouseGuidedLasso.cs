using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseGuidedLasso : MonoBehaviour
{
    [SerializeField] Transform _mouseTarget;
    [SerializeField] Transform _lassoVisual;
    [Space]
    public int pullForce;
    public float lassoSize = 0.0f;
    public float maxSize = 5.0f;
    public float growRate = 0.01f;
    [Space] //hide these later, only used for testing
    public bool isMovingLasso = false;
    public bool isChangingSize = false;

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
            //ToggleLasso();
            StartLasso();
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            StopLasso();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGrow();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopGrow();
        }

        //change cursor state
        UpdateCursorState();

        //enable/disable visual
        //RefreshVisuals();

        //move target to mouse position
        if (_lassoVisual != null && _mouseTarget != null)
        {
            if (_lassoVisual.gameObject.activeInHierarchy) _lassoVisual.position = Utility.GetMouseHitPoint(0f);
            if (_mouseTarget.gameObject.activeInHierarchy) _mouseTarget.position = Utility.GetMouseHitPoint(0f);
        }
        #endregion

        #region Scale lasso
        if (isChangingSize)
        {
            Debug.Log("Resizing");
            lassoSize += growRate * Time.deltaTime;

            if(lassoSize >= maxSize)
            {
                lassoSize = maxSize;
            }

            ScaleLasso(lassoSize);
        }
        #endregion
    }
    #region Helpers
    private void UpdateCursorState()
    {
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
    }
    /// <summary>
    /// Activate/deactivate visuals based on bools.
    /// </summary>
    private void RefreshVisuals()
    {
        if (_mouseTarget == null || _lassoVisual == null) return;

        // activate/deactivate visuals based on bools
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
    /// <summary>
    /// Resize the visual for lasso range (to be changed during polish).
    /// </summary>
    /// <param name="scale"></param>
    private void ScaleLasso(float scale)
    {
        Transform child = _lassoVisual.GetChild(0);

        if (child.localScale.x == scale) return;

        Vector3 adjustedScale = new Vector3(scale, child.localScale.y, scale);
        child.localScale = adjustedScale;
    }
    #endregion

    #region Lasso State
    private void ToggleLasso()
    {
        if (isChangingSize) return;

        //switch lasso bool
        isMovingLasso = !isMovingLasso;

        RefreshVisuals();
    }
    private void StartLasso()
    {
        if (isChangingSize) return;

        //switch lasso bool
        isMovingLasso = true;

        RefreshVisuals();
    }
    private void StopLasso()
    {
        //do action if ready
        if (isChangingSize) DoLassoAction();

        //switch lasso bool
        isMovingLasso = false;
        isChangingSize = false;

        RefreshVisuals();
    }
    private void StartGrow()
    {
        if (!isMovingLasso) return;

        //switch changing bool
        isChangingSize = true;

        //start changing lasso size
        lassoSize = 0.01f;

        RefreshVisuals();
    }
    private void StopGrow()
    {
        //do action if ready
        if(isMovingLasso) DoLassoAction();

        if (!isMovingLasso) return;

        //switch changing bool
        isChangingSize = false;
        isMovingLasso = false;

        RefreshVisuals();
    }
    #endregion

    #region Lasso Action
    private void DoLassoAction()
    {
        Debug.LogWarning("DoLassoAction()");

        //copied from LassoProjectile but substituting variables
        Collider[] _enemiesInRange = Physics.OverlapSphere(transform.position, lassoSize);
        for (int i = 0; i < _enemiesInRange.Length; i++)
        {
            if (_enemiesInRange[i].gameObject.TryGetComponent(out Chainable chainable))
            {
                _enemiesInRange[i].transform.position = Vector3.MoveTowards(_enemiesInRange[i].transform.position, 
                    Utility.GetMouseHitPoint(0f), pullForce * Time.deltaTime);
            }
        }
    }
    #endregion
}
