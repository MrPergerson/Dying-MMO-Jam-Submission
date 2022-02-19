using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class StorySystem : MonoBehaviour
{
    [Title("Setup")]
    [SerializeField, LabelText("Start Chapter")] private int _currentChapter = 1;
    //[SerializeField] private int startCondition = 1;

    [Title("Status")]
    [SerializeField, ReadOnly, LabelText("Current Chapter")] private string currentChapterName;
    [SerializeField, ReadOnly] public storyState CurrentStoryState = storyState.NotStarted;
        

    [Title("Chapters In Story")][SerializeField]
    List<Chapter> chapters = new List<Chapter>();

    public delegate void ChapterChanged();
    public event ChapterChanged onChapterChanged;

    private int CurrentChapter { get { return _currentChapter; } set { _currentChapter = value; } } 
    public enum storyState { NotStarted, TransitioningToNextChapter, Paused, Ended }

    private void InitializeStory()
    {
        CurrentChapter -= 2;
        if (CurrentChapter < -1) CurrentChapter = -1;
        if (CurrentChapter >= chapters.Count) CurrentChapter = chapters.Count - 1;

        CurrentStoryState = storyState.NotStarted;

        foreach (var chapter in chapters)
        {
            chapter.gameObject.SetActive(false);
        }

    }

    public void InitializeStoryConditions()
    {
        chapters[CurrentChapter].InitializeCurrentStoryCondition();
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
            InitializeStory();
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

        SceneManager.Instance.LoadLevel("GameOver");
    }
}
