using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField] StorySystem storySystem;

    [SerializeField] private TextAsset inkfile;

    private void Awake()
    {
        // Check that there is only one Dialogue Manager in scene
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Dialogue Manager detected");
        }
        instance = this;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        if (storySystem == null) Debug.LogError(this + ": Story system is null");

        storySystem.onChapterChanged += PrintSuccess;
        storySystem.StartStory();
        inkfile = storySystem.GetCurrentChapterInkFile();
        SendInkToDialogueManager();
    }

    private void PrintSuccess()
    {
        print("The chapter has changed!");
        if (inkfile != storySystem.GetCurrentChapterInkFile())
        {
            inkfile = storySystem.GetCurrentChapterInkFile();
            SendInkToDialogueManager();
        }
        
    }

    private void SendInkToDialogueManager()
    {
        DialogueManagerAS2.GetInstance().ChangeInkJSON(inkfile);
    }
}
