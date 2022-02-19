using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        GameManager.GetInstance().GoToMainMenu();
    }

    public void Quit()
    {
        Debug.LogWarning("Quit method does not run in editor. Must build and run application to test Quit method.");
        Application.Quit();
    }
}
