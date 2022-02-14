using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [SerializeField] private GameObject loadingCanvas;

    private Scene activeScene;

    private bool _isLoadingScene = false;
    private bool isProcessingLoad = false;

    public delegate void LevelLoaded();
    public event LevelLoaded onLevelLoaded;

    public bool IsLoadingScene { get { return _isLoadingScene; } private set { _isLoadingScene = value; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            InitializeScenes();
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    public void InitializeScenes()
    {
        activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        //var gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(LoadSceneAsync("Managers", true));
        StartCoroutine(LoadSceneAsync("UI System", true));
        var managerScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Managers");
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(this.gameObject, managerScene);
        //UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameManager.gameObject, managerScene);
    }

    /*

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }

    public void LoadScene(string name, bool isAdditive)
    {
        StartCoroutine(LoadSceneAsync(name, isAdditive));
    }

    */

    public void LoadLevel(string name)
    {

        StartCoroutine(LoadLevelAsync(name));

    }

    IEnumerator LoadLevelAsync(string name)
    {
        isProcessingLoad = true;
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(name);

        StartCoroutine(UnLoadSceneAsync(activeScene.name));

        while (isProcessingLoad)
        {
            yield return null;
        }

        isProcessingLoad = true;
        StartCoroutine(LoadSceneAsync(name, true));

        while (isProcessingLoad)
        {
            yield return null;
        }

        onLevelLoaded?.Invoke();
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


}
