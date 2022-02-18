using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : Manager
{
    private static StoryManager Instance;

    StorySystem story;
    private bool isReadyToChangeScene;

    public static StoryManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
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
        story = FindObjectOfType<StorySystem>();
    }

    public override bool IsReadyToChangeScene()
    {
        return isReadyToChangeScene;
    }

    public override void OnNewLevelLoaded()
    {
        story.InitializeStoryConditions();
    }

    public override void OnSceneChangeRequested()
    {
        isReadyToChangeScene = true;
    }
}
