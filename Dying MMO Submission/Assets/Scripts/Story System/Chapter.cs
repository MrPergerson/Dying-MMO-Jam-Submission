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
    [SerializeField] private TextAsset InkFile;

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
    }

    public void StartChapter()
    {
        StopAllCoroutines();
        onChapterStarted?.Invoke();
        StartCoroutine(CheckForEndingTransition());
    }

    public void EndChapter()
    {
        onChapterEnded?.Invoke();
        StopAllCoroutines();
    }

    IEnumerator CheckForEndingTransition()
    {
        StartCoroutine(CheckForConditionsMet());

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

            if (transitionsToNextChapter[currentIndex].ConditionMet())
                currentIndex++;

            return false;
        });

        nextChapterTransitionPassed = true;
        
    }


}
