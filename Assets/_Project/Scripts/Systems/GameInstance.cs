using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance;

    //only for during development stuff
    public bool debug = false;

    #region Game State Stuff

    public bool isPaused = false;

    #endregion

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(this);
        #endregion
    }
    private void Update()
    {
        if (debug)
        {
            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                ReloadLevel();
            }
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    #region Level Loading
    public static void ReloadLevel()
    {
        Loader.Load(SceneManager.GetActiveScene().name);
    }
    public static void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
