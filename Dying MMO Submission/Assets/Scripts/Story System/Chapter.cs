using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Chapter : MonoBehaviour
{
    
    [TableColumnWidth(50, Resizable = false), ReadOnly, SerializeField] 
    private bool playing;
    [TableColumnWidth(150, Resizable = false),SerializeField]
    private string _name;
    [SerializeField] private List<StoryCondition> transitionsToNextChapter;
    [SerializeField] private TextAsset inkFile;

    public delegate void ChapterStarted();
    public delegate void ChapterEnded();
    public event ChapterStarted onChapterStarted;
    public event ChapterEnded onChapterEnded;

    [Title("Debug")]
    [SerializeField, ReadOnly] private bool nextChapterTransitionPassed = false;

    public string Name { get { return _name; } }

    private void Start()
    {
        if(transitionsToNextChapter == null)
        {
            Debug.LogError(this.gameObject + ": transitionsToNextChapter is null");
        }
        else if (transitionsToNextChapter.Count == 0)
        {
            Debug.LogError(this.gameObject + ": No ending transitions found");
        }

        foreach(var transition in transitionsToNextChapter)
        {
            if (transition == null) Debug.LogError(this + ": There is a null transition in " + gameObject.name + "'s transition list");
        }

        if (inkFile == null) Debug.LogError(this + ": No inkfile has been assigned to this chapter");

        if(!playing)
        {
            gameObject.SetActive(false);
        }

        ResetAllTransitions();
    }

    public void StartChapter()
    {
        playing = true;
        StopAllCoroutines();
        ResetAllTransitions();
        onChapterStarted?.Invoke();
        StartCoroutine(CheckForEndingTransition());
    }

    public void EndChapter()
    {
        playing = false;
        onChapterEnded?.Invoke();
        StopAllCoroutines();
    }

    public void ResetChapter()
    {
        nextChapterTransitionPassed = false;


        ResetAllTransitions();


    }

    IEnumerator CheckForEndingTransition()
    {
        if(!nextChapterTransitionPassed)
        {
            StartCoroutine(CheckForConditionsMet());
        }

        yield return new WaitUntil(() => nextChapterTransitionPassed);

        EndChapter();
    }

    IEnumerator CheckForConditionsMet()
    {
        int currentIndex = 0;

        yield return new WaitUntil(() =>
        {
            if (currentIndex >= transitionsToNextChapter.Count)
                return true;

            transitionsToNextChapter[currentIndex].gameObject.SetActive(true);
            transitionsToNextChapter[currentIndex].SetAsCurrentCondition(true);

            if (transitionsToNextChapter[currentIndex].IsConditionMet())
            {
                transitionsToNextChapter[currentIndex].SetAsCurrentCondition(false);
                transitionsToNextChapter[currentIndex].gameObject.SetActive(false);
                currentIndex++;
            }

            return false;
        });

        nextChapterTransitionPassed = true;
        
    }

    private void ResetAllTransitions()
    {
        foreach (var condition in transitionsToNextChapter)
        {
            condition.ResetCondition();
            condition.gameObject.SetActive(false);
        }
    }

    public TextAsset GetInkFile()
    {
        return inkFile;
    }


}
