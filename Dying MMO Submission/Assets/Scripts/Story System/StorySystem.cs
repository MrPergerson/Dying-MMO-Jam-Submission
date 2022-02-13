using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StorySystem : MonoBehaviour
{
    [SerializeField, ReadOnly, LabelText("Current Chapter")] private string currentChapterName;
    [SerializeField, ReadOnly] storyState CurrentStoryState = storyState.NotStarted;
    //[TableList]
    [SerializeField]
    List<Chapter> chapters = new List<Chapter>();

    public delegate void ChapterChanged();
    public event ChapterChanged onChapterChanged;

    private int CurrentChapter { get; set; } 
    private enum storyState { NotStarted, TransitioningToNextChapter, Paused, Ended }

    private void ResetStory()
    {
        CurrentChapter = -1;
        CurrentStoryState = storyState.NotStarted;

        foreach (var chapter in chapters)
        {
            chapter.gameObject.SetActive(false);
        }

    }

    private void BeginChapter()
    {
        SetStoryDetails(storyState.TransitioningToNextChapter, chapters[CurrentChapter].Name);

        onChapterChanged?.Invoke();

        chapters[CurrentChapter].onChapterEnded += TransitionToNextChapter;
        chapters[CurrentChapter].StartChapter();
    }

    private void TransitionToNextChapter()
    {
        CurrentChapter++;

        if (CurrentStoryState != storyState.NotStarted) CloseLastChapter();

        if (CurrentChapter >= chapters.Count) 
            EndStory();
        else
        {
            chapters[CurrentChapter].gameObject.SetActive(true);
            BeginChapter();
        }
    }

    private void SetStoryDetails(storyState state, string name)
    {
        CurrentStoryState = state;
        currentChapterName = name;
    }

    private void CloseLastChapter()
    {
        //chapters[CurrentChapter-1].EndChapter();
        chapters[CurrentChapter-1].onChapterEnded -= TransitionToNextChapter;
        chapters[CurrentChapter-1].gameObject.SetActive(false);
    }

    public TextAsset GetCurrentChapterInkFile()
    {
        if(CurrentStoryState != storyState.NotStarted && CurrentStoryState != storyState.Ended)
        {
            return chapters[CurrentChapter].GetInkFile();
        }
        else
        {
            Debug.LogError(this + ": Request was made to get the current chapter's ink file, but the story has either not started or has ended");
            return null;
        }
    }

    [Button("Start Story")]
    public void StartStory()
    {
        if(CurrentStoryState == storyState.NotStarted || CurrentStoryState == storyState.Ended)
        {
            ResetStory();
            TransitionToNextChapter();
        }
        else
        {
            Debug.LogError(this + ": The story must end before it can start");
        }

    }


    [Button("End Story")]
    public void EndStory()
    {
        currentChapterName = "";
        CurrentStoryState = storyState.Ended;

        foreach (var chapter in chapters)
        {
            chapter.onChapterEnded -= TransitionToNextChapter;
            chapter.ResetChapter();
            chapter.gameObject.SetActive(false);
        }

        print("Story Ended");
    }
}