using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    [SerializeField] StorySystem storySystem;
    [SerializeField] DialogueManagerAS2 dialogueManager;

    [SerializeField, ReadOnly] private TextAsset inkfile;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            dialogueManager = FindObjectOfType<DialogueManagerAS2>();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.Instance.onLevelLoaded += SetupLevel;
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

    private void SetupLevel()
    {
        storySystem = FindObjectOfType<StorySystem>();
        if (storySystem == null) Debug.LogError(this + ": Story system is null");

        storySystem.onChapterChanged += PrintSuccess;
        storySystem.StartStory();
        inkfile = storySystem.GetCurrentChapterInkFile();
        SendInkToDialogueManager();


    }
}
