using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    TabGroup tabGroup;

    public Image background;

    private Transform _content;

    public delegate void TabSelected(TabSelectButton tab);
    public delegate void TabDeselected(TabSelectButton tab);
    public event TabSelected onTabSelected;
    public event TabDeselected onTabDeselected;

    public Transform Content { get { return _content; } set { _content = value; } }

    private void Awake()
    {
        tabGroup = GetComponentInParent<TabGroup>();
        if (tabGroup == null) Debug.LogError(this + ": TabSelectButton is active but has no TabGroup as a parent");

        background = GetComponent<Image>();
    }

    private void Start()
    {
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        onTabSelected?.Invoke(this);
    }

    public void Deselect()
    {
        onTabDeselected?.Invoke(this);
    }

}
