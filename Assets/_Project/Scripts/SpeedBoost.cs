using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private int _moveSpeedIncrease;
    [SerializeField] private float _duration;
    [SerializeField] private GameObject _artToDisable;
    [SerializeField] ParticleSystem _pickUpEffect;
    [SerializeField] AudioClip _pickUpSound;

    private Collider _collider;
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
    }

    public IEnumerator ApplySpeedBoost(ThirdPersonController controller)
    {
        //soft disable
        _collider.enabled = false;
        _artToDisable.SetActive(false);

        //apply speed
        controller.MoveSpeed += _moveSpeedIncrease;
        controller.SprintSpeed += _moveSpeedIncrease;
        if(_pickUpEffect != null) Instantiate(_pickUpEffect, transform.position, transform.rotation);
        if (_pickUpSound != null) Instantiate(_pickUpSound, transform.position, transform.rotation);

        yield return new WaitForSeconds(_duration);

        //return speed to normal and destory
        controller.MoveSpeed -= _moveSpeedIncrease;
        controller.SprintSpeed -= _moveSpeedIncrease;
        Destroy(gameObject);
    }
}
