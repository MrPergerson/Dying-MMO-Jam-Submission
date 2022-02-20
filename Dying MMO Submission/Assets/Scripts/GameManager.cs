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
    [SerializeField, ReadOnly] private bool persistentSceneLoaded = false;

    StorySystem storySystem;
    //DialogueManagerAS2 dialogueManager;
    SceneManager sceneManager;

    [Title("Managers")]
    [SerializeField, ReadOnly] List<Manager> managerList = new List<Manager>();

    [Title("Debug")]
    [SerializeField, ReadOnly] private TextAsset inkfile;
    [SerializeField, ReadOnly] private bool isReadyToChangeScene = false;

    public delegate void ManagersInitialized();
    public event ManagersInitialized onAllManagersInitialized;

    public static GameManager GetInstance()
    {
        return Instance;
    }

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
        GetInkAndSendToDialogueManager();
    }

    public override void OnNewLevelLoaded()
    {
        // when level is loaded//

        SetupAllLevelExits();
        isReadyToChangeScene = false;
    }

    public override void OnSceneChangeRequested()
    {
        CloseAllLevelExits();
        //storySystem.EndStory();
        isReadyToChangeScene = true;
    }

    public override bool IsReadyToChangeScene()
    {
        return isReadyToChangeScene;
    }

    /// Added
    public void GetInkAndSendToDialogueManager()
    {
        storySystem = FindObjectOfType<StorySystem>();
        if (storySystem != null)
        {
            storySystem.onChapterChanged += PrintSuccess;
            storySystem.StartStory();
            inkfile = storySystem.GetCurrentChapterInkFile();
            SendInkToDialogueManager();
        }
    }

    public StorySystem GetStorySystemReferenceFromGameManager()
    {
        return storySystem;
    }
    ///

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

    private void SetupAllLevelExits()
    {
        var levelExits = FindObjectsOfType<LevelExit>();
        var levelDirs = FindObjectsOfType<LevelDirectoryExit>();

        foreach (var exit in levelExits)
        {
            exit.onGoToLevel += sceneManager.LoadLevel;
            
        }

        foreach (var exit in levelDirs)
        {
            exit.onGoToLevel += sceneManager.LoadLevel;

        }
    }

    private void CloseAllLevelExits()
    {
        var levelExits = FindObjectsOfType<LevelExit>();
        var levelDirs = FindObjectsOfType<LevelDirectoryExit>();

        foreach (var exit in levelExits)
        {
            exit.onGoToLevel -= sceneManager.LoadLevel;
        }

        foreach (var exit in levelDirs)
        {
            exit.onGoToLevel -= sceneManager.LoadLevel;

        }
    }

    #region UI

    public void StartGameHere()
    {
        SetupPersistentScene();
        //NotifyAllManagersOfLevelChange();
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
        if (startGameOnAwake)
        {
            sceneManager.onPersistentSceneLoaded += NotifyAllManagersOfLevelChange;
            startGameOnAwake = false;
        }
        sceneManager.LoadPersistentScene();
    }

    public void InitializeAllManagers()
    {
        persistentSceneLoaded = true;

        DialogueManagerAS2.GetInstance().AwakeManager();
        StoryManager.GetInstance().AwakeManager();
        GameManager.Instance.AwakeManager();
        SceneManager.Instance.AwakeManager();
        ActionBarManager.GetInstance().AwakeManager();
        
        var managers = FindObjectsOfType<Manager>();

        foreach(var manager in managers)
        {
           // manager.AwakeManager();
            managerList.Add(manager);
        }
        
        onAllManagersInitialized?.Invoke();
    }

    public bool AllScenesReadyToChangeScene()
    {
        var count = 0;
        foreach (var manager in managerList)
        {
            if (manager.IsReadyToChangeScene())
                count++;
        }
        return count == managerList.Count;
    }

    public void NotifyAllManagersOfSceneChangeRequest()
    {
        foreach (var manager in managerList)
        {
            manager.OnSceneChangeRequested();
        }
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
