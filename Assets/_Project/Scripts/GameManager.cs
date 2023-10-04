using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private UniversalRendererData _renderData;
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _deathMenuUI;
    public static GameManager Instance;

    private StarterAssets.StarterAssetsInputs inputs;
    private bool isPasued = false;

    private void Awake()
    {
        /*#region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        #endregion*/
    }

    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        inputs = FindObjectOfType<StarterAssets.StarterAssetsInputs>();
    }

    private void Update()
    {
        if(isPasued == false && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause();
        }
        else if (isPasued = true && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Unpause();
        }
    }
    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        inputs.cursorInputForLook = false;
        isPasued = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void Unpause()
    {
        inputs.cursorInputForLook = true;
        isPasued = false;
        _pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void ContinueButton()
    {
        Unpause();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public static void ReloadLevel()
    {
        Loader.Load(SceneManager.GetActiveScene().name);
    }
    public static void QuitGame()
    {
        Application.Quit();
    }

    public void Death()
    {
        _deathMenuUI.SetActive(true);
        inputs.cursorInputForLook = false;
        isPasued = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
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
