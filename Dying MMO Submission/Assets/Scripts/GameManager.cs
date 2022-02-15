using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : Manager
{
    private static GameManager Instance;
    [Title("Game Setup")]
    [SerializeField] GameObject SceneManagerPrefab;
    [SerializeField] bool startGameOnAwake = false;
    [SerializeField, ReadOnly]private bool persistentSceneLoaded = false;

    StorySystem storySystem;
    DialogueManagerAS2 dialogueManager;
    SceneManager sceneManager;

    [Title("Managers")]
    [SerializeField, ReadOnly] List<Manager> managerList = new List<Manager>();

    [Title("Debug")]
    [SerializeField, ReadOnly] private TextAsset inkfile;

    public delegate void ManagersInitialized();
    public event ManagersInitialized onAllManagersInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (startGameOnAwake)
                StartGameHere();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void AwakeManager()
    {
        sceneManager.onLevelLoaded += NotifyAllManagersOfLevelChange;

        storySystem = FindObjectOfType<StorySystem>();
        if (storySystem == null) Debug.LogError(this + ": Story system is null");

        storySystem.onChapterChanged += PrintSuccess;
        storySystem.StartStory();
        inkfile = storySystem.GetCurrentChapterInkFile();
        SendInkToDialogueManager();
    }

    public override void OnNewLevelLoaded()
    {
        // when level is loaded//
        
    }

    private void PrintSuccess()
    {
        //print("The chapter has changed!");
        if (inkfile != storySystem.GetCurrentChapterInkFile())
        {
            inkfile = storySystem.GetCurrentChapterInkFile();
            SendInkToDialogueManager();
        }
    }

    private void SendInkToDialogueManager()
    {
        if (!DialogueManagerAS2.GetInstance())
            Debug.LogError(this + ": GameManager cannot find Dialogue Manager. Dialogue Manager might not be in scene.");

        DialogueManagerAS2.GetInstance().ChangeInkJSON(inkfile);
    }

    #region UI

    public void StartGameHere()
    {
        SetupPersistentScene();
    }

    public void StartGameAtLevel(string levelName)
    {
        StartCoroutine( StartGameAtLevelAsync(levelName));
    }

    IEnumerator StartGameAtLevelAsync(string levelName)
    {
        if (!persistentSceneLoaded)
        {
            SetupPersistentScene();
        }

        while (!persistentSceneLoaded)
            yield return null;

        sceneManager.LoadLevel(levelName);

    }

    [Button()]
    public void GoToMainMenu()
    {
        sceneManager.ReturnToMainMenu();
    }

    #endregion

    #region Scene Management
    public void SetupPersistentScene()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        if (sceneManager == null)
        {
            sceneManager = Instantiate(SceneManagerPrefab).GetComponent<SceneManager>();
            if (sceneManager == null) Debug.LogError(this + ": sceneManager prefab does not have a SceneManager component attached");
        }

        sceneManager.onPersistentSceneLoaded += InitializeAllManagers;
        sceneManager.LoadPersistentScene();
    }

    public void InitializeAllManagers()
    {
        persistentSceneLoaded = true;

        var managers = FindObjectsOfType<Manager>();

        foreach(var manager in managers)
        {
            manager.AwakeManager();
            managerList.Add(manager);
        }

        onAllManagersInitialized?.Invoke();
    }

    public void NotifyAllManagersOfLevelChange()
    {
        foreach (var manager in managerList)
        {
            manager.OnNewLevelLoaded();
        }
    }
    #endregion

}
