using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }

    private void Start()
    {
        CreateTab("test");
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
            if (content.tag == "UI_TabPage_Content")
            {
                tabSelectButton.Content = content;
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



    
}
