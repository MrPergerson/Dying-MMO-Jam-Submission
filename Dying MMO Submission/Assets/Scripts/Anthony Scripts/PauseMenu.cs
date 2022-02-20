using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    private bool isPaused = false;

    public GameObject pauseMenu;
    public AudioMixer audioMixerMaster;

    private PlayerControls controls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!pauseMenu)
        {
            Debug.LogError(this + ": Pause Menu object is empty.");
        }
        else
        {
            pauseMenu.SetActive(false);
        }

        controls = new PlayerControls();
    }

    private void Start()
    {
        controls.Main.EndGame.performed += Pause;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
        }
    }

    public void MainMenu()
    {
        isPaused = false;
        GameManager.GetInstance().GoToMainMenu();
    }

    public void Quit()
    {
        Debug.LogWarning("Quit method does not run in editor. Must build and run application to test Quit method.");
        Application.Quit();
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixerMaster.SetFloat("MasterVolume", volume);
    }

    public void SetAMBVolume(float volume)
    {
        audioMixerMaster.SetFloat("AMBVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixerMaster.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixerMaster.SetFloat("SFXVolume", volume);
    }
}
