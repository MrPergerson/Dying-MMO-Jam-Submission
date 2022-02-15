using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

/*
 * TODO: Add loading screen
 */

public class SceneManager : Manager
{
    public static SceneManager Instance;

    [Title("Loading Screen")]
    [SerializeField] private GameObject loadingCanvas;

    private Scene _currentLevelScene;
    private bool _isLoadingScene = false;
    private bool isProcessingLoad = false;

    public delegate void LevelLoaded();
    public event LevelLoaded onLevelLoaded;

    public delegate void PersistentSceneLoaded();
    public event PersistentSceneLoaded onPersistentSceneLoaded;

    public bool IsLoadingScene { get { return _isLoadingScene; } private set { _isLoadingScene = value; } }

    public Scene CurrentLevel { get { return _currentLevelScene; } private set { _currentLevelScene = value; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public override void AwakeManager()
    {
        //print(" scene manager ready");
        //print(CurrentLevel.name);
    }

    public override void OnNewLevelLoaded()
    {
        //print("new level loaded");
        //print(CurrentLevel.name);
    }

    [Button()]
    public void LoadLevel(string name)
    {
        CurrentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        StartCoroutine(LoadLevelAsync(name, CurrentLevel.name));

    }

    IEnumerator LoadLevelAsync(string nextLevel, string currentLevel)
    {
        isProcessingLoad = true;
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(nextLevel);


        StartCoroutine(UnLoadSceneAsync(CurrentLevel.name));

        while (isProcessingLoad)
        {
            yield return null;
        }

        isProcessingLoad = true;
        StartCoroutine(LoadSceneAsync(nextLevel, true));

        while (isProcessingLoad)
        {
            yield return null;
        }

        onLevelLoaded?.Invoke();

        var newLevel = UnityEngine.SceneManagement.SceneManager.GetSceneByName(nextLevel);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(newLevel);

    }

    IEnumerator LoadSceneAsync(string name, bool isAdditive = false)
    {
        var loadMode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, loadMode);
        //loadingCanvas.SetActive(true);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isProcessingLoad = false;

        //loadingCanvas.SetActive(false); , LoadSceneMode.Additive
    }

    IEnumerator UnLoadSceneAsync(string name)
    {
        AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
        //loadingCanvas.SetActive(true);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        isProcessingLoad = false;
        //loadingCanvas.SetActive(false); , LoadSceneMode.Additive
    }

    public void LoadPersistentScene()
    {
        StartCoroutine(LoadPersistentSceneAsync());
    }

    IEnumerator LoadPersistentSceneAsync()
    {
        isProcessingLoad = true;
        CurrentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        StartCoroutine(LoadSceneAsync("PersistentGameObjects", true));

        while (isProcessingLoad)
        {
            yield return null;
        }

        var persistentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("PersistentGameObjects");
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(persistentScene);

        var gameManager = FindObjectOfType<GameManager>();
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameManager.gameObject, persistentScene);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.gameObject, persistentScene);

        UnityEngine.SceneManagement.SceneManager.SetActiveScene(CurrentLevel);

        onPersistentSceneLoaded?.Invoke();
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }

}
