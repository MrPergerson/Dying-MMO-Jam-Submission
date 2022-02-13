using System.Collections;
using Ink.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class DialogueManagerAS2 : MonoBehaviour
{
    private static DialogueManagerAS2 instance;

    [Header("Globals Ink File")]
    [SerializeField] private TextAsset globalVarInkFile;

    private Story currentStory;
    public bool chatIsPlaying { get; private set; }

    public float chatDelayNum = 2f;
    private float chatDelay;

    [SerializeField, ReadOnly] private TextAsset inkJSON;

    private DialogueVariables dialogueVariables;

    private TabGroup tabGroup;

    private bool playInGameDialogue = false;
    public TextMeshProUGUI inGameDialogueText;
    private TextAsset inGameDialogueInkJSON;

    private void Awake()
    {
        // Check that there is only one Dialogue Manager in scene
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Dialogue Manager detected");
        }
        instance = this;

        chatDelay = chatDelayNum;

        dialogueVariables = new DialogueVariables(globalVarInkFile);
    }

    public static DialogueManagerAS2 GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        tabGroup = GameObject.FindGameObjectWithTag("UI_MessageBox").GetComponent<TabGroup>();
        if (!tabGroup)
        {
            Debug.LogError(this + ": TabGroup component could not found in Scene.");
        }
        inGameDialogueText.gameObject.SetActive(false);
        chatIsPlaying = false;
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

    [Button("Enter Dialogue Mode")]
    private void EnterDialogueMode()
    {
        if (inkJSON == null)
        {
            Debug.LogError(this + ": no ink file to read. Did not recieve ink from Game Manager.");
        }
        else
        {
            currentStory = new Story(inkJSON.text);
            chatIsPlaying = true;
            dialogueVariables.StartListening(currentStory);
            CheckWhatToPlay();
            ContinueStory();
        }
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);
        inGameDialogueText.gameObject.SetActive(false);
        chatIsPlaying = false;
    }

    private void CheckWhatToPlay()
    {
        if (!inGameDialogueInkJSON)
        {
            playInGameDialogue = true;
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (playInGameDialogue)
            {
                //Display text to In-Game Dialogue
                PlayInGameDialogue();
            }
            else
            {
                //Display text to Message Box chat
                tabGroup.DisplayChatLine(currentStory);
                tabGroup.DisplayChoices(currentStory);
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void PlayInGameDialogue()
    {
        string storyText = currentStory.Continue();

        // If chatLineText is empty/new line, destroy it
        if (storyText != "\n" && storyText != null)
        {
            inGameDialogueText.text = storyText;
            inGameDialogueText.gameObject.SetActive(true);
        }
        
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    public Ink.Runtime.Object GetVariable(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogError(this + ": Could not get the variable, " + variableName + ", from the globalVariable list.");
        }
        return variableValue;
    }

    public void ChangeInkJSON(TextAsset inkJSONFile)
    {
        inkJSON = inkJSONFile;
        //EnterDialogueMode();
    }

    public void ChangeInGameDialogueInk(TextAsset inkJSONFile)
    {
        if (!inkJSONFile)
        {
            inGameDialogueInkJSON = inkJSONFile;
        }
        else
        {
            inGameDialogueInkJSON = null;
        }
    }
}
