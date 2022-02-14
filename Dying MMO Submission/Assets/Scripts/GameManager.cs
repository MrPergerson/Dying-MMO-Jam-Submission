using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField] StorySystem storySystem;

    [SerializeField, ReadOnly] private TextAsset inkfile;

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
        if (!DialogueManagerAS2.GetInstance())
            Debug.LogError(this + ": GameManager cannot find Dialogue Manager. Dialogue Manager might not be in scene.");

        DialogueManagerAS2.GetInstance().ChangeInkJSON(inkfile);
    }
}
