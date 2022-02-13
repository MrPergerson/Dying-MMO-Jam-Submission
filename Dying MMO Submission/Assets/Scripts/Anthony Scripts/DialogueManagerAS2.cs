using System.Collections;
using Ink.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class DialogueManagerAS2 : MonoBehaviour
{
    private static DialogueManagerAS2 instance;

    [Header("Globals Ink File")]
    [SerializeField] private TextAsset globalVarInkFile;

    private Story currentStory;
    public bool chatIsPlaying { get; private set; }

    public float chatDelayNum = 1f;
    private float chatDelay;

    [SerializeField, ReadOnly] private TextAsset inkJSON;

    private DialogueVariables dialogueVariables;

    private TabGroup tabGroup;
    [ReadOnly] public string userName = "Public";

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

            ContinueStory();
        }
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);
        chatIsPlaying = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            tabGroup.DisplayChatLine(currentStory, userName);
            tabGroup.DisplayChoices(currentStory);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
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
    }

    [Button("Change Username")]
    public void ChangeUserName()
    {
        userName = Random.Range(0, 100).ToString();
    }
}
