using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] StorySystem storySystem;

    private void Start()
    {
        if (storySystem == null) Debug.LogError(this + ": Story system is null");

        storySystem.onChapterChanged += PrintSuccess;
        storySystem.StartStory();
        TextAsset inkfile = storySystem.GetCurrentChapterInkFile();
    }

    private void PrintSuccess()
    {
        print("The chapter has changed!");
    }


}
