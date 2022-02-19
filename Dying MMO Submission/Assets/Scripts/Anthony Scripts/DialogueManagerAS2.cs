using System.Collections;
using Ink.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class DialogueManagerAS2 : Manager
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

    [SerializeField, ReadOnly] private TextMeshProUGUI inGameDialogueText;

    private void Awake()
    {
        // Check that there is only one Dialogue Manager in scene
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Dialogue Manager detected");
            
        }
        instance = this;
        dialogueVariables = new DialogueVariables(globalVarInkFile);
    }

    public static DialogueManagerAS2 GetInstance()
    {
        return instance;
    }

    public override void AwakeManager()
    {
        // Called by the game manager
        // This will call when the PersistentGameObjects scene has loaded. However, a level may not be loaded at this time.
        // Only expect to get references to gameobjects within the PersistentGameObjects scene. (Assuming this gameObject is in the Persistent scene)

        isAwake = true;

        //print("dialogue system ready");

        chatDelay = chatDelayNum;

        


        tabGroup = FindObjectOfType<TabGroup>();
        //tabGroup.gameObject.SetActive(true);

        if (!tabGroup)
            Debug.LogError(this + ": TabGroup component could not found in Scene.");
        /*
        inGameDialogueText = GameObject.FindGameObjectWithTag("UI_OnScreen_Text").GetComponent<TextMeshProUGUI>();
        if (!inGameDialogueText)
            Debug.LogError(this + ": Can't find On Screen Text object in Persistent Scene.");

        inGameDialogueText.gameObject.SetActive(false);
        */
        chatIsPlaying = false;
    }

    public override void OnNewLevelLoaded()
    {
        // called every time a new level is loaded
        // You should be able to safetly reference all gameObjects from all active scenes in this function.
        // Scenes loaded that are not considered as levels, such as MainMenu and PersistentGameObjects, will not trigger this
    }

    public override void OnSceneChangeRequested()
    {
        // The scene is about the change
        // SceneManger will wait until IsReadyToChangeScene returns true before changing the scene
    }

    public override bool IsReadyToChangeScene()
    {
        // will get checked multiple times
        // to don't put anything performance heavy in here
        return true;
    }

    private void Update()
    {
        if(isAwake)
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
    }

    [Button("Enter Dialogue Mode")]
    public void EnterDialogueMode()
    {
        //Debug.Log("Enter Dialogue called with current story as " + currentStory);
        if (inkJSON == null)
        {
            Debug.LogError(this + ": no ink file to read. Did not recieve ink from Game Manager.");
        }
        else
        {
            currentStory = new Story(inkJSON.text);
            chatIsPlaying = true;
            //Debug.Log("Dialogue vars is null: " + (dialogueVariables == null));
            dialogueVariables.StartListening(currentStory);
            ContinueStory();
        }
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);
        //inGameDialogueText.gameObject.SetActive(false);
        chatIsPlaying = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //inGameDialogueText.gameObject.SetActive(false);
            tabGroup.DisplayChatLine(currentStory);
            tabGroup.DisplayChoices(currentStory);
            StartCoroutine(ClearLastHighlightedChoice());
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    public void PlayInGameDialogue(string speaker, string inGameText)
    {
        //inGameDialogueText.text = speaker + ": " + inGameText;
        //inGameDialogueText.gameObject.SetActive(true);
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

    private IEnumerator ClearLastHighlightedChoice()
    {
        // event system requires we clear it to avoid carrying over the Highlighted button animation
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1f); //IEnumerator needs a yeid return, so it wont throw error
    }


}
