using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject settingsObject;

    public AudioMixer audioMixerMaster;

    void Start()
    {
        if (!settingsObject)
        {
            Debug.LogError(this + ": Settings object is missing.");
        }
        else if(settingsObject.activeSelf)
        {
            settingsObject.SetActive(false);
        }
    }

    public void Settings()
    {
        mainButtons.SetActive(false);
        settingsObject.SetActive(true);
    }

    public void Back()
    {
        settingsObject.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void Quit()
    {
        Debug.LogWarning(this + ": cannot quit game in editor. Must build and run game to test Quit.");
        Application.Quit();
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
