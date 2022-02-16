using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class TabGroup : MonoBehaviour
{
    List<TabSelectButton> tabButtons;

    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public List<GameObject> pages;

    public GameObject tabButton;
    public GameObject tabPage;

    private Transform tabArea;
    private Transform tabContent;

    private TabSelectButton selectedTab;

    public Dictionary<string, TabSelectButton> tabDictionary { get; private set; }
    private string currentTab = "Public";
    private string currentUserName;
    private bool playInGameDialogue;

    [Header("Chat Line UI")]
    [SerializeField] private GameObject chatLineObj;

    [Header("Chat Choices UI")]
    [SerializeField] private Button[] choiceButtons;
    private TextMeshProUGUI[] choiceButtonText;

    private void Awake()
    {
        tabButtons = new List<TabSelectButton>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var obj = transform.GetChild(i);

            if (obj.tag == "UI_MessageBox_TabArea")
                tabArea = obj;
            else if (obj.tag == "UI_MessageBox_TabContent")
                tabContent = obj;
        }

        if (tabArea == null) Debug.LogError(this + ": Could not find TabArea gameobject in children. Was the UI_MessageBox_TabArea tag assigned to it?");
        if (tabContent == null) Debug.LogError(this + ": Could not find TabContent gameobject in children. Was the UI_MessageBox_TabContent tag assigned to it?");

        tabDictionary = new Dictionary<string, TabSelectButton>();
    }

    private void Start()
    {
        CreateTab(currentTab);
        SetChoicesText();
    }

    private void SetChoicesText()
    {
        /* Create Choice text array of TMPro GUI with equal to number of Chat Choices that can seen on screen
         * This array will hold indiviaual choice text ("Yes", "No", etc.)
         */

        // Make choiceButtonText array length equal to number of buttons
        choiceButtonText = new TextMeshProUGUI[choiceButtons.Length];
        int index = 0;
        foreach (Button choice in choiceButtons)
        {
            // Get each TextMeshProUGUI component and save in choiceButtonText array
            choiceButtonText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            // Make buttons invisible
            choice.gameObject.SetActive(false);
            index++;
        }
    }

    public void DisplayChoices(Story story)
    {
        List<Choice> currentChoices = story.currentChoices;

        if (currentChoices.Count > choiceButtons.Length)
        {
            Debug.LogError(this + " DisplayChoices: too many choices for UI, add more Choice button UI");
        }

        int index = 0;
        //enable and initialize the choices up to amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choiceButtons[index].gameObject.SetActive(true);
            choiceButtonText[index].text = choice.text;
            index++;
        }
        // go through remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        //StartCoroutine(ClearEventSystemChoices());
    }

    private IEnumerator ClearEventSystemChoices()
    {
        // event system requires we clear it first, then wait
        // for at least one frame before we set current selected objects    
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
    }

    public void OnClickChoice(int buttonIndex)
    {
        DialogueManagerAS2.GetInstance().MakeChoice(buttonIndex);
    }

    private void ResetTabs()
    {
        foreach(var tab in tabButtons)
        {
            if (selectedTab != null && selectedTab == tab) continue;

            tab.background.color = tabIdle;
        }
    }

    private void SelectTab(TabSelectButton tabButton)
    {
        if (selectedTab != null) selectedTab.Deselect();

        selectedTab = tabButton;

        selectedTab.Select();

        ResetTabs();
        tabButton.background.color = tabActive;
        int tabIndex = tabButton.transform.GetSiblingIndex();
        for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
        {
            if (pageIndex == tabIndex)
            {
                pages[pageIndex].SetActive(true);
            }
            else
            {
                pages[pageIndex].SetActive(false);
            }
        }
    }

    public TabSelectButton CreateTab(string name)
    {
        var newTabPage = Instantiate(tabPage,tabContent);
        newTabPage.gameObject.name = name + " Tab Content";
        pages.Add(newTabPage.gameObject);

        var newTabButton = Instantiate(tabButton, tabArea);

        TextMeshProUGUI tabTitle = newTabButton.GetComponentInChildren<TextMeshProUGUI>();
        if (tabTitle)
            tabTitle.text = name;
        else
            Debug.LogError(this + ": Could not find the TextMeshProUGUI component in the children of the tab button");

        TabSelectButton tabSelectButton = null;
        if(newTabButton.TryGetComponent(out tabSelectButton))
        {
            tabButtons.Add(tabSelectButton);
            SelectTab(tabSelectButton);

            //this is bad, if this is a problem, then TabPage should have a class that does this. 
            var content = newTabPage.transform.GetChild(0).GetChild(0);
            if (content.transform.tag == "UI_TabPage_Content")
            {
                tabSelectButton.Content = content.transform;
                /* ADDED
                 * tabDictionary.Add
                 */
                tabDictionary.Add(name, tabSelectButton);
            }
            else
            {
                Debug.LogError(this + ": Could not assign a content gameObject to tabSelectButton. Was the UI_TabPage_Content tag assigned to it?" +
                    " if the problem persists, see code for details");
            }
        }

        return tabSelectButton;
    }

    public void Subscribe(TabSelectButton tabButton)
    {
        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabSelectButton tabButton)
    {
        ResetTabs();
        if(selectedTab == null || selectedTab != tabButton)
        {
            tabButton.background.color = tabHover;
        }
    }

    public void OnTabExit(TabSelectButton tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabSelectButton tabButton)
    {
        SelectTab(tabButton);
    }

    /* ADDED
    * GetTabContentTransform
    * DisplayChatLine
    * 
    */
    public Transform GetTabContentTransform(string keyName)
    {
        if (tabDictionary.ContainsKey(keyName))
        {
            return tabDictionary[keyName].Content;
        }

        //Debug.LogError(this + ": Could not find " + keyName + " among Tabs. Check spelling.");
        return null;
    }

    public void DisplayChatLine(Story story)
    {
        GameObject chatLine = Instantiate(chatLineObj);
        
        string chatLineText = story.Continue();
        
        // If chatLineText is empty/new line, destroy it
        if (chatLineText == "\n" || chatLineText == null)
        {
            Destroy(chatLine);
        }
        else
        {
            HandleTags(story.currentTags);
            InsertChatLine(chatLine, chatLineText);
        }
    }

    public void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            if (tag.Contains(":"))
            {
                string[] splitTag = tag.Split(':');
                if (splitTag.Length != 2)
                {
                    Debug.LogError(this + ": could not parse tag correctly. Please check tags in Ink.");
                }
                string key = splitTag[0].Trim();
                string value = splitTag[1].Trim();

                if (key == "ingame")
                {
                    playInGameDialogue = true;
                }
                else
                {
                    currentTab = value;
                }
            }
            currentUserName = tag;
        }
    }

    public void InsertChatLine(GameObject chatObject, string chatText)
    {
        if (playInGameDialogue)
        {
            print(playInGameDialogue);
            //ignore Tabs and play single dialogue to in-game in dialogue
            DialogueManagerAS2.GetInstance().PlayInGameDialogue(currentUserName, chatText);
            playInGameDialogue = false;
        }
        else
        {
            if (!GetTabContentTransform(currentTab))
                CreateTab(currentTab);

            chatObject.GetComponent<TextMeshProUGUI>().text = "["+ currentUserName +"]" + ": " + chatText;
            chatObject.transform.SetParent(GetTabContentTransform(currentTab));
            //print(currentTab);
        }
    }
}
