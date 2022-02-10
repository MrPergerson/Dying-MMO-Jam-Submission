using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueManagerAs : MonoBehaviour
{
    private static DialogueManagerAs instance;

    [Header("Chat Line UI")]
    [SerializeField] private GameObject[] chatLineParent;
    [SerializeField] private GameObject chatLineObj;
    [SerializeField] private Transform chatLinePos; 
    [SerializeField] private TextMeshProUGUI chatText;

    [Header("Chat Choices UI")]
    [SerializeField] private GameObject[] chatChoices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool chatIsPlaying { get; private set; }

    public float chatDelayNum = 1f;
    private float chatDelay;

    public TextAsset inkJSON;

    private void Awake()
    {
        // Check that there is only one Dialogue Manager in scene
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Dialogue Manager detected");
        }
        instance = this;

        chatDelay = chatDelayNum;
    }

    // Get Dialogue Manager Instance
    public static DialogueManagerAs GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        chatIsPlaying = false;

        // Create Choice text array of TMPro GUI with equal to number of Chat Choices that can seen on screen
        // This array will hold indiviaual choice text ("Yes", "No", etc.)
        choicesText = new TextMeshProUGUI[chatChoices.Length];
        int index = 0;
        foreach (GameObject choice in chatChoices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            choice.SetActive(false);
            index++;
        }

        EnterDialogueMode(inkJSON);
    }

    private void Update()
    {
        if (!chatIsPlaying)
        {
            return;
        }

        // Check there are no choices left before continuing the story
        if (currentStory.currentChoices.Count == 0)
        {
            if (chatDelay <= 0)
            {
                chatDelay = chatDelayNum;
                ContinueStory();
            }
            else
            {
                chatDelay -= Time.deltaTime;
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        chatIsPlaying = true;

        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        //print("exiting");
        yield return new WaitForSeconds(0.2f);
        chatIsPlaying = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            GameObject chatLine = Instantiate(chatLineObj, chatLinePos.position, Quaternion.identity);
            chatLine.GetComponent<TextMeshProUGUI>().text = currentStory.Continue();
            string chatLineText = chatLine.GetComponent<TextMeshProUGUI>().text;
            if (chatLineText == "\n")
            {
                Destroy(chatLine);
            }
            chatLine.transform.SetParent(chatLineParent[0].transform);
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    public void ContinueStory(InputAction.CallbackContext context)
    {
        if (currentStory.currentChoices.Count != 0) { return; }

        if (currentStory.canContinue)
        {
            chatText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > chatChoices.Length)
        {
            Debug.LogError(this.gameObject + " DisplayChoices: too many choices for UI, add more Choice button UI");
        }

        int index = 0;
        //enable and initialize the choices up to amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            chatChoices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < chatChoices.Length; i++)
        {
            chatChoices[i].gameObject.SetActive(false);
        }

        //StartCoroutine(SelectFirstChoice());
    }

    /*private IEnumerator SelectFirstChoice()
    {
        // event system requires we clear it first, then wait
        // for at least one frame before we set current selected objects    
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(chatChoices[0].gameObject);
    }*/

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
