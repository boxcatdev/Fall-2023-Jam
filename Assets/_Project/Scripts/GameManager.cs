using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private UniversalRendererData _renderData;
    [SerializeField] private GameObject _pauseMenuUI;

    private StarterAssets.StarterAssetsInputs inputs;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        inputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
    }

    private void Update()
    {
        if(Keyboard.current.escapeKey.isPressed)
        {
            Pause();
        }
    }
    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void ContinueButton()
    {
        _pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    /*public void PixilationToggle()
    {
        if(_renderData.rendererFeatures[2].isActive)
        {
            _renderData.rendererFeatures[2].SetActive(false);
        }
        else if (!_renderData.rendererFeatures[2].isActive)
        {
            _renderData.rendererFeatures[2].SetActive(true);
        }
    }*/
}
