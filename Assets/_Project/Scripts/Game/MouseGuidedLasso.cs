using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseGuidedLasso : MonoBehaviour
{
    [SerializeField] ParticleSystem _ringEffect;
    [SerializeField] Transform _mouseTarget;
    [SerializeField] Transform _lassoVisual;
    [Space]
    [SerializeField] private int pullForce;
    [SerializeField] private float lassoSize = 0.0f;
    [SerializeField] private float maxSize = 5.0f;
    [SerializeField] private float growRate = 0.01f;

    [Header("Cooldown")]
    [SerializeField] Image _cooldownDisplay;
    [SerializeField] float _cooldownTime;
    
    private bool _canLasso = true;
    private bool _cooldownStarted = false;
    private float _countdown;

    //private these later, only used for testing
    private bool isMovingLasso = false;
    private bool isChangingSize = false;

    private StarterAssets.StarterAssetsInputs inputs;
    private WaveManager waveManager;

    private void Awake()
    {
        inputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
        waveManager = FindObjectOfType<WaveManager>();
    }
    private void Start()
    {
        RefreshVisuals();

        if (_mouseTarget != null) _mouseTarget.SetParent(null);
        if (_lassoVisual != null) _lassoVisual.SetParent(null);

        //ParticleSystem.ShapeModule ps = _ringEffect.GetComponent<ParticleSystem>().shape;
    }
    private void Update()
    {
        #region Lasso position
        //inputs
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (_canLasso)
            {
                StartLasso();
                StartShrink();
            }
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (_canLasso)
            {
                StopLasso();
                StopShrink();
                _canLasso = false;
            }
        }

        //change cursor state
        if(waveManager != null)
            if(waveManager.wavesInProgress) UpdateCursorState();

        //move target to mouse position
        if (_lassoVisual != null && _mouseTarget != null && _ringEffect != null)
        {
            if (_lassoVisual.gameObject.activeInHierarchy) _lassoVisual.position = Utility.GetMouseHitPoint(1f);
            if (_mouseTarget.gameObject.activeInHierarchy) _mouseTarget.position = Utility.GetMouseHitPoint(0f);
        }
        #endregion

        #region Scale lasso
        if (isChangingSize)
        {
            Debug.Log("Resizing");
            lassoSize -= growRate * Time.deltaTime;

            /*if(lassoSize >= maxSize)
            {
                lassoSize = maxSize;
            }*/
            if(lassoSize <= 0)
            {
                lassoSize = 0;
                StopLasso();
                StopShrink();
                _canLasso = false;
            }

            ScaleLasso(lassoSize);
        }
        #endregion

        #region Cooldown

        if(_canLasso == false)
        {
            if(_cooldownStarted == false)
            {
                //start cooldown
                _cooldownStarted = true;
                _countdown = _cooldownTime;
            }

            if(_countdown >= 0)
            {
                _countdown -= Time.deltaTime;
            }
            else
            {
                //_countdown = _cooldownTime;
                if(_cooldownStarted == true && _canLasso == false)
                {
                    //end lasso
                    _canLasso = true;
                    _cooldownStarted = false;
                }
            }

            RefreshCooldownDisplay();
        }

        #endregion
    }
    #region Helpers
    private void RefreshCooldownDisplay()
    {
        if(_cooldownDisplay != null)
        {
            _cooldownDisplay.fillAmount = 1 - (_countdown / _cooldownTime);
        }
    }
    private void UpdateCursorState()
    {
        //change cursor state
        if (isMovingLasso)
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Confined;
            //if (inputs.cursorInputForLook == true) inputs.cursorInputForLook = false;
        }
        else
        {
            if (Cursor.lockState == CursorLockMode.Confined) Cursor.lockState = CursorLockMode.Locked;
            //if (inputs.cursorInputForLook == false) inputs.cursorInputForLook = true;
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
        ParticleSystem.ShapeModule ps = _ringEffect.shape;
        ps.radius = scale;
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
    private void StartShrink()
    {
        if (!isMovingLasso) return;

        //switch changing bool
        isChangingSize = true;

        //start changing lasso size
        lassoSize = maxSize;

        RefreshVisuals();
    }
    private void StopShrink()
    {
        //do action if ready
        //if (isMovingLasso) DoLassoAction();

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
        Collider[] _enemiesInRange = Physics.OverlapSphere(Utility.GetMouseHitPoint(0f), lassoSize);
        for (int i = 0; i < _enemiesInRange.Length; i++)
        {
            if (_enemiesInRange[i].gameObject.TryGetComponent(out Enemy enemy))
            {
                Debug.Log("enemy hit");
                _enemiesInRange[i].transform.position = Vector3.MoveTowards(_enemiesInRange[i].transform.position, 
                    Utility.GetMouseHitPoint(0f), pullForce * Time.deltaTime);
            }
        }
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(Utility.GetMouseHitPoint(0f), lassoSize);
    }
}
