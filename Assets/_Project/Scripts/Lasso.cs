using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Lasso : MonoBehaviour
{
    [SerializeField] private GameObject _lasso;
    [SerializeField] private GameObject _lassoSpawnPoint;
    [SerializeField] private Slider _chargeBar;
    [SerializeField] private float _maxCharge;
    [SerializeField] private float _chargePerSecond;
    [SerializeField] private float _chargeMultiplier;
    [SerializeField] private float _rechargeTime;
    [SerializeField] private Slider _cooldownSlider;

    private float _currentCharge = 0;
    private bool _chargingUp = true;
    private Vector3 _spawnPos;
    private Quaternion _spawnRotation;
    private float _currentRechargeProgess = 0;
    private bool _isReady = true;

    // Start is called before the first frame update
    void Start()
    {
        _chargeBar.maxValue = _maxCharge;
        _cooldownSlider.maxValue = _rechargeTime;
        _cooldownSlider.value = _rechargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _spawnPos = _lassoSpawnPoint.transform.position;
        _spawnRotation = _lassoSpawnPoint.transform.rotation;
        if (Mouse.current.rightButton.isPressed && _isReady)
        {
            //made slider appear
            if(_chargeBar.gameObject.activeInHierarchy == false)
            {
                _chargeBar.gameObject.SetActive(true);
            }
            //charging bar up and down while right mouse is held
            ChargingBar();
        }
        // on right mouse release throw lasso
        if(Mouse.current.rightButton.wasReleasedThisFrame && _isReady)
        {
            ThrowLasso();
            //_chargeBar.gameObject.SetActive(false);
            _chargeBar.value = 0;
            _currentCharge = 0;
            _cooldownSlider.value = 0;
            _isReady = false;
        }
        //if on cooldown countdown
        if(!_isReady)
        {
            _currentRechargeProgess += Time.deltaTime;
            _cooldownSlider.value = _currentRechargeProgess;
            if (_currentRechargeProgess >= _rechargeTime)
            {
                _isReady = true;
                _currentRechargeProgess = 0;
            }
        }
    }

    private void ChargingBar()
    {
        if (_chargingUp)
        {
            _currentCharge += _chargePerSecond * Time.deltaTime;
            _chargeBar.value = _currentCharge;
            if (_currentCharge >= _maxCharge)
            {
                _chargingUp = false;
            }
        }
        else if (!_chargingUp)
        {
            _currentCharge -= _chargePerSecond * Time.deltaTime;
            _chargeBar.value = _currentCharge;
            if (_currentCharge <= 0)
            {
                _chargingUp = true;
            }
        }
    }

    private void ThrowLasso()
    {
        Quaternion newLassoRotation = Quaternion.Euler(0, Camera.main.transform.localRotation.y, Camera.main.transform.localRotation.z);
        var lasso = Instantiate(_lasso, _spawnPos, newLassoRotation);
        var lassoRb = lasso.GetComponent<Rigidbody>();
        lassoRb.AddRelativeForce(Vector3.forward * _currentCharge * _chargeMultiplier);
    }

}
