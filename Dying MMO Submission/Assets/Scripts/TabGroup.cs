using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    List<TabSelectButton> tabButtons;

    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public List<GameObject> pages;

    private TabSelectButton selectedTab;

    public void Subscribe(TabSelectButton tabButton)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabSelectButton>();
        }

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
        if (selectedTab != null) selectedTab.Deselect();

        selectedTab = tabButton;

        selectedTab.Select();

        ResetTabs();
        tabButton.background.color = tabActive;
        int tabIndex = tabButton.transform.GetSiblingIndex();
        for(int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
        {
            if(pageIndex == tabIndex)
            {
                pages[pageIndex].SetActive(true);
            }
            else
            {
                pages[pageIndex].SetActive(false);
            }
        }

    }

    private void ResetTabs()
    {
        foreach(var tab in tabButtons)
        {
            if (selectedTab != null && selectedTab == tab) continue;

            tab.background.color = tabIdle;
        }
    }
}
