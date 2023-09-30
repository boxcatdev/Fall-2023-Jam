using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private int _moveSpeedIncrease;
    [SerializeField] private float _duration;
    [SerializeField] private GameObject _orbToDisable;
    [SerializeField] ParticleSystem _pickUpEffect;
    [SerializeField] AudioSource _pickUpSound;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Vector3 _rotationDirection;

    private Collider _collider;
    private MeshRenderer _artToDisable;
    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonController controller = other.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            StartCoroutine(ApplySpeedBoost(controller));
        }
    }
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _artToDisable = GetComponent<MeshRenderer>();
        _pickUpSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(_rotateSpeed * _rotationDirection * Time.deltaTime);
    }

    public IEnumerator ApplySpeedBoost(ThirdPersonController controller)
    {
        //soft disable
        _collider.enabled = false;
        _artToDisable.enabled = false;
        _orbToDisable.SetActive(false);

        //apply speed
        controller.MoveSpeed += _moveSpeedIncrease;
        controller.SprintSpeed += _moveSpeedIncrease;
        if(_pickUpEffect != null) Instantiate(_pickUpEffect, transform.position, transform.rotation);
        if (_pickUpSound != null) _pickUpSound.Play();

        yield return new WaitForSeconds(_duration);

        //return speed to normal and destory
        controller.MoveSpeed -= _moveSpeedIncrease;
        controller.SprintSpeed -= _moveSpeedIncrease;
        Destroy(gameObject);
    }
}
