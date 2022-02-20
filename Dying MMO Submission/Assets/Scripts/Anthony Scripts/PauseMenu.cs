using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    private bool isPaused = false;

    public GameObject pauseMenu;
    public AudioMixer audioMixer;

    private PlayerControls controls;
    [SerializeField] private List<Slider> sliders = new List<Slider>();

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
        foreach (Slider slider in sliders)
        {
            if (slider.name.Contains("Master"))
            {
                float val = 0;
                audioMixer.GetFloat("MasterVolume", out val);
                slider.value = val;
            }
            else if (slider.name.Contains("Music"))
            {
                float val = 0;
                audioMixer.SetFloat("MusicVolume", val);
                slider.value = val;
            }
            else if (slider.name.Contains("Ambience"))
            {
                float val = 0;
                audioMixer.SetFloat("AMBVolume", val);
                slider.value = val;
            }
            else if (slider.name.Contains("SFX"))
            {
                float val = 0;
                audioMixer.SetFloat("SFXVolume", val);
                slider.value = val;
            }
        }
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
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetAMBVolume(float volume)
    {
        audioMixer.SetFloat("AMBVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
}
