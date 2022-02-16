using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirectoryExit : MonoBehaviour, ILevelExit, ISelectable
{

    [SerializeField] private GameObject _levelDirUIElement;
    private bool windowIsActive = false;

    public event ILevelExit.GoToLevel onGoToLevel;

    public GameObject LevelDirUIElement { get { return _levelDirUIElement; } set { _levelDirUIElement = value; } }

    public void TravelToLevel(string name)
    {
        onGoToLevel?.Invoke(name);
    }

    public void Select()
    {
        if (LevelDirUIElement)
        {
            windowIsActive = !windowIsActive;
            LevelDirUIElement.SetActive(windowIsActive);
        }
        else
        {
            Debug.LogError(this + " has no reference to a UI level directory window");
        }
    }
}
